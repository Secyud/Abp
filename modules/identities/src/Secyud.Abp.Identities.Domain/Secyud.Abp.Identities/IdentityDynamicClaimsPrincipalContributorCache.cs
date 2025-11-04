using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Secyud.Abp.Identities;

public class IdentityDynamicClaimsPrincipalContributorCache(
    IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache,
    ICurrentTenant currentTenant,
    IdentityUserManager userManager,
    IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory,
    IOptions<AbpClaimsPrincipalFactoryOptions> abpClaimsPrincipalFactoryOptions,
    IOptions<IdentityDynamicClaimsPrincipalContributorCacheOptions> cacheOptions)
    : ITransientDependency
{
    public ILogger<IdentityDynamicClaimsPrincipalContributorCache> Logger { get; set; } = NullLogger<IdentityDynamicClaimsPrincipalContributorCache>.Instance;

    protected IDistributedCache<AbpDynamicClaimCacheItem> DynamicClaimCache { get; } = dynamicClaimCache;
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;
    protected IdentityUserManager UserManager { get; } = userManager;
    protected IUserClaimsPrincipalFactory<IdentityUser> UserClaimsPrincipalFactory { get; } = userClaimsPrincipalFactory;
    protected IOptions<AbpClaimsPrincipalFactoryOptions> AbpClaimsPrincipalFactoryOptions { get; } = abpClaimsPrincipalFactoryOptions;
    protected IOptions<IdentityDynamicClaimsPrincipalContributorCacheOptions> CacheOptions { get; } = cacheOptions;

    public virtual async Task<AbpDynamicClaimCacheItem?> GetAsync(Guid userId, Guid? tenantId = null)
    {
        Logger.LogDebug("Get dynamic claims cache for user: {UserId}", userId);

        if (AbpClaimsPrincipalFactoryOptions.Value.DynamicClaims.IsNullOrEmpty())
        {
            var emptyCacheItem = new AbpDynamicClaimCacheItem();
            await DynamicClaimCache.SetAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId), emptyCacheItem, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheOptions.Value.CacheAbsoluteExpiration
            });

            return emptyCacheItem;
        }

        using (CurrentTenant.Change(tenantId))
        {
            return await DynamicClaimCache.GetOrAddAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId), async () =>
            {
                Logger.LogDebug("Filling dynamic claims cache for user: {UserId}", userId);

                var user = await UserManager.GetByIdAsync(userId);
                var principal = await UserClaimsPrincipalFactory.CreateAsync(user);

                var dynamicClaims = new AbpDynamicClaimCacheItem();
                foreach (var claimType in AbpClaimsPrincipalFactoryOptions.Value.DynamicClaims)
                {
                    var claims = principal.Claims.Where(x => x.Type == claimType).ToList();
                    if (claims.Any())
                    {
                        dynamicClaims.Claims.AddRange(claims.Select(claim => new AbpDynamicClaim(claimType, claim.Value)));
                    }
                    else
                    {
                        dynamicClaims.Claims.Add(new AbpDynamicClaim(claimType, null));
                    }
                }

                return dynamicClaims;
            }, () => new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheOptions.Value.CacheAbsoluteExpiration
            });
        }
    }

    public virtual async Task ClearAsync(Guid userId, Guid? tenantId = null)
    {
        Logger.LogDebug("Remove dynamic claims cache for user: {UserId}", userId);
        await DynamicClaimCache.RemoveAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(userId, tenantId));
    }
}
