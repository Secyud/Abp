using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Secyud.Abp.Identities.Settings;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities.Session;

public class IdentitySessionManager(
    IIdentitySessionRepository identitySessionRepository,
    ICurrentUser currentUser,
    IDistributedCache<IdentitySessionCacheItem> cache,
    IUnitOfWorkManager unitOfWorkManager,
    ISettingProvider settingProvider,
    IdentityDynamicClaimsPrincipalContributorCache identityDynamicClaimsPrincipalContributorCache)
    : DomainService
{
    protected IIdentitySessionRepository IdentitySessionRepository { get; } = identitySessionRepository;
    protected ICurrentUser CurrentUser { get; } = currentUser;
    protected IDistributedCache<IdentitySessionCacheItem> Cache { get; } = cache;
    protected IUnitOfWorkManager UnitOfWorkManager { get; } = unitOfWorkManager;
    protected ISettingProvider SettingProvider { get; } = settingProvider;

    protected IdentityDynamicClaimsPrincipalContributorCache IdentityDynamicClaimsPrincipalContributorCache { get; } =
        identityDynamicClaimsPrincipalContributorCache;

    public virtual async Task<IdentitySession> CreateAsync(
        string sessionId,
        string device,
        string deviceInfo,
        Guid userId,
        Guid? tenantId,
        string clientId,
        string ipAddresses,
        bool setLastAccessed = false)
    {
        Check.NotNullOrWhiteSpace(sessionId, nameof(sessionId));
        Check.NotNullOrWhiteSpace(device, nameof(device));

        using (CurrentTenant.Change(tenantId))
        {
            var session = await IdentitySessionRepository.FindAsync(sessionId);
            if (session == null)
            {
                Logger.LogDebug(
                    "Creating identity session for session id: {SessionId}, device: {Device}, user id: {UserId}, tenant id: {TenantId}, client id: {ClientId}",
                    sessionId, device, userId, tenantId, clientId);
                DateTime? lastAccessed = setLastAccessed ? Clock.Now : null;
                session = await IdentitySessionRepository.InsertAsync(new IdentitySession(
                    GuidGenerator.Create(),
                    sessionId,
                    device,
                    deviceInfo,
                    userId,
                    tenantId,
                    clientId,
                    ipAddresses,
                    Clock.Now,
                    lastAccessed
                ));
            }

            var preventConcurrentLoginBehaviour = await IdentitiesPreventConcurrentLoginBehaviourSettingHelper.Get(SettingProvider);
            switch (preventConcurrentLoginBehaviour)
            {
                case IdentitiesPreventConcurrentLoginBehaviour.LogoutFromSameTypeDevices:
                    await RevokeAllAsync(userId, device, session.Id);
                    break;

                case IdentitiesPreventConcurrentLoginBehaviour.LogoutFromAllDevices:
                    await RevokeAllAsync(userId, session.Id);
                    break;
            }

            return session;
        }
    }

    public virtual async Task UpdateAsync(IdentitySession session)
    {
        await IdentitySessionRepository.UpdateAsync(session);
    }

    public virtual async Task<List<IdentitySession>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? userId = null,
        string? device = null,
        string? clientId = null)
    {
        var sessions = await IdentitySessionRepository.GetListAsync(
            sorting,
            maxResultCount,
            skipCount,
            userId,
            device,
            clientId
        );

        var sessionCacheItems = await Cache.GetManyAsync(sessions.Select(s => s.SessionId).ToArray());
        var changedSessions = new List<IdentitySession>();
        foreach (var cacheItem in sessionCacheItems.Where(x => x.Value != null))
        {
            var session = sessions.FirstOrDefault(s => s.SessionId == cacheItem.Key);
            if (session != null && await UpdateSessionFromCacheItemAsync(session, cacheItem.Value))
            {
                changedSessions.Add(session);
            }
        }

        if (changedSessions.Any())
        {
            await IdentitySessionRepository.UpdateManyAsync(changedSessions);
        }

        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = $"{nameof(IdentitySession.LastAccessed)} desc";
        }

        if (sorting.Contains(nameof(IdentitySession.LastAccessed), StringComparison.OrdinalIgnoreCase))
        {
            sessions = sorting.Contains("desc")
                ? sessions.OrderByDescending(s => s.LastAccessed).ToList()
                : sessions.OrderBy(s => s.LastAccessed).ToList();
        }

        return sessions;
    }

    public virtual async Task<IdentitySession> GetAsync(Guid id)
    {
        return (await UpdateSessionFromCacheAsync(await IdentitySessionRepository.GetAsync(id)))!;
    }

    public virtual async Task<IdentitySession?> FindAsync(Guid id)
    {
        return await UpdateSessionFromCacheAsync(await IdentitySessionRepository.FindAsync(id));
    }

    public virtual async Task<IdentitySession?> FindAsync(string sessionId, bool updateFromCache = true)
    {
        var session = await IdentitySessionRepository.FindAsync(sessionId);
        return updateFromCache ? await UpdateSessionFromCacheAsync(session) : session;
    }

    public virtual async Task<bool> ExistAsync(Guid id)
    {
        return await IdentitySessionRepository.ExistAsync(id);
    }

    public virtual async Task<bool> ExistAsync(string sessionId)
    {
        return await IdentitySessionRepository.ExistAsync(sessionId);
    }

    public virtual async Task<IdentitySession?> UpdateSessionFromCacheAsync(string sessionId, IdentitySessionCacheItem sessionCacheItem)
    {
        return await UpdateSessionFromCacheAsync(await IdentitySessionRepository.FindAsync(sessionId), sessionCacheItem);
    }

    protected virtual async Task<IdentitySession?> UpdateSessionFromCacheAsync(IdentitySession? session, IdentitySessionCacheItem? sessionCacheItem = null)
    {
        if (session == null)
        {
            return null;
        }

        sessionCacheItem ??= await Cache.GetAsync(session.SessionId);
        if (sessionCacheItem != null && await UpdateSessionFromCacheItemAsync(session, sessionCacheItem))
        {
            session = await IdentitySessionRepository.UpdateAsync(session);
        }

        return session;
    }

    protected virtual Task<bool> UpdateSessionFromCacheItemAsync(IdentitySession? session, IdentitySessionCacheItem? sessionCacheItem)
    {
        if (session == null)
        {
            return Task.FromResult(false);
        }

        if (sessionCacheItem == null)
        {
            return Task.FromResult(false);
        }

        var changed = false;
        if (sessionCacheItem.CacheLastAccessed != null && (session.LastAccessed == null || sessionCacheItem.CacheLastAccessed > session.LastAccessed))
        {
            session.UpdateLastAccessedTime(sessionCacheItem.CacheLastAccessed);
            changed = true;
        }

        if (!sessionCacheItem.IpAddress.IsNullOrWhiteSpace())
        {
            var ipAddresses = session.GetIpAddresses().ToList();
            ipAddresses.RemoveAll(x => x == sessionCacheItem.IpAddress);
            ipAddresses.Add(sessionCacheItem.IpAddress);
            session.SetIpAddresses(ipAddresses);
            changed = true;
        }

        return Task.FromResult(changed);
    }

    public virtual async Task RevokeAsync(Guid id)
    {
        var session = await IdentitySessionRepository.FindAsync(id);
        if (session == null)
        {
            return;
        }

        await RevokeAsync(session);
    }

    public virtual async Task RevokeAsync(string sessionId)
    {
        var session = await IdentitySessionRepository.FindAsync(sessionId);
        if (session == null)
        {
            return;
        }

        await RevokeAsync(session);
    }

    public virtual async Task RevokeAsync(IdentitySession session)
    {
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(session.UserId, session.TenantId);
        try
        {
            await IdentitySessionRepository.DeleteAsync(session.Id);
        }
        catch (AbpDbConcurrencyException)
        {
            // Session may be deleted by another request, so we can ignore this exception.
        }
    }

    public virtual async Task RevokeAllAsync(Guid userId, Guid? exceptSessionId = null)
    {
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(userId);
        await IdentitySessionRepository.DeleteAllAsync(userId, exceptSessionId);
    }

    public virtual async Task RevokeAllAsync(Guid userId, string device, Guid? exceptSessionId = null)
    {
        await IdentityDynamicClaimsPrincipalContributorCache.ClearAsync(userId);
        await IdentitySessionRepository.DeleteAllAsync(userId, device, exceptSessionId);
    }

    public virtual async Task DeleteAllAsync(TimeSpan inactiveTimeSpan)
    {
        await IdentitySessionRepository.DeleteAllAsync(inactiveTimeSpan);
    }
}