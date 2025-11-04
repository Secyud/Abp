using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Timing;

namespace Secyud.Abp.Identities.Session;

public class IdentitySessionChecker(
    IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOption,
    IdentitySessionManager identitySessionManager,
    IDistributedCache<IdentitySessionCacheItem> cache,
    IClock clock,
    IWebClientInfoProvider webClientInfoProvider,
    IOptions<IdentitySessionCheckerOptions> options)
    : ITransientDependency
{
    public ILogger<IdentitySessionChecker> Logger { get; set; } = NullLogger<IdentitySessionChecker>.Instance;

    protected IOptions<AbpClaimsPrincipalFactoryOptions> AbpClaimsPrincipalFactoryOptions { get; } = abpClaimsPrincipalFactoryOption;
    protected IdentitySessionManager IdentitySessionManager { get; } = identitySessionManager;
    protected IDistributedCache<IdentitySessionCacheItem> Cache { get; } = cache;
    protected IClock Clock { get; } = clock;
    protected IWebClientInfoProvider WebClientInfoProvider { get; } = webClientInfoProvider;
    protected IOptions<IdentitySessionCheckerOptions> Options { get; } = options;

    public virtual async Task<bool> IsValidateAsync(string sessionId)
    {
        if (!AbpClaimsPrincipalFactoryOptions.Value.IsDynamicClaimsEnabled)
        {
            Logger.LogDebug("Dynamic claims is disabled, The SessionId({SessionId}) will not be checked.", sessionId);
            return true;
        }

        if (sessionId.IsNullOrWhiteSpace())
        {
            Logger.LogWarning("SessionId is null or empty cannot be checked.");
            return false;
        }

        var sessionCacheItem = await Cache.GetOrAddAsync(sessionId, async () =>
        {
            Logger.LogDebug("Get SessionId({SessionId}) from IdentitySessionManager.", sessionId);

            var session = await IdentitySessionManager.FindAsync(sessionId, false);
            if (session == null)
            {
                Logger.LogWarning("Could not find SessionId({SessionId}) in the database.", sessionId);
                return null!;
            }

            Logger.LogDebug("Found SessionId({SessionId}) in the database.", sessionId);
            return new IdentitySessionCacheItem
            {
                Id = session.Id,
                SessionId = session.SessionId
            };
        });

        if (sessionCacheItem == null)
        {
            await Cache.RemoveAsync(sessionId);
            return false;
        }

        sessionCacheItem.CacheLastAccessed = Clock.Now;
        sessionCacheItem.IpAddress = WebClientInfoProvider.ClientIpAddress;
        sessionCacheItem.HitCount++;

        Logger.LogDebug(
            "SessionId({SessionId}) found in cache, Updating hit count({HitCount}), " +
            "last access time({CacheLastAccessed}) and IP address({IpAddress}).",
            sessionId, sessionCacheItem.HitCount, sessionCacheItem.CacheLastAccessed, sessionCacheItem.IpAddress
        );

        if (sessionCacheItem.HitCount == 1)
        {
            Logger.LogDebug($"Updating the session from cache on the first check.");
            await IdentitySessionManager.UpdateSessionFromCacheAsync(sessionId, sessionCacheItem);
        }
        else if (sessionCacheItem.HitCount > Options.Value.UpdateSessionAfterCacheHit)
        {
            Logger.LogDebug("Update the session from cache because reached the maximum cache hit count({hitCount}).",
                Options.Value.UpdateSessionAfterCacheHit);
            sessionCacheItem.HitCount = 0;
            await IdentitySessionManager.UpdateSessionFromCacheAsync(sessionId, sessionCacheItem);
        }

        await Cache.SetAsync(sessionId, sessionCacheItem);

        return true;
    }
}