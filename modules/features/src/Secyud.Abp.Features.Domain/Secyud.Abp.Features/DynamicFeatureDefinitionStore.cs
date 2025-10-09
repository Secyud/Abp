using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Features;
using Volo.Abp.Threading;

namespace Secyud.Abp.Features;

[Dependency(ReplaceServices = true)]
public class DynamicFeatureDefinitionStore(
    IFeatureGroupDefinitionRecordRepository featureGroupRepository,
    IFeatureDefinitionRecordRepository featureRepository,
    IFeatureDefinitionSerializer featureDefinitionSerializer,
    IDynamicFeatureDefinitionStoreInMemoryCache storeCache,
    IDistributedCache distributedCache,
    IOptions<AbpDistributedCacheOptions> cacheOptions,
    IOptions<FeaturesOptions> featuresOptions,
    IAbpDistributedLock distributedLock)
    : IDynamicFeatureDefinitionStore, ITransientDependency
{
    protected IFeatureGroupDefinitionRecordRepository FeatureGroupRepository { get; } = featureGroupRepository;
    protected IFeatureDefinitionRecordRepository FeatureRepository { get; } = featureRepository;
    protected IFeatureDefinitionSerializer FeatureDefinitionSerializer { get; } = featureDefinitionSerializer;
    protected IDynamicFeatureDefinitionStoreInMemoryCache StoreCache { get; } = storeCache;
    protected IDistributedCache DistributedCache { get; } = distributedCache;
    protected IAbpDistributedLock DistributedLock { get; } = distributedLock;
    public FeaturesOptions FeaturesOptions { get; } = featuresOptions.Value;
    protected AbpDistributedCacheOptions CacheOptions { get; } = cacheOptions.Value;

    public virtual async Task<FeatureDefinition?> GetOrNullAsync(string name)
    {
        if (!FeaturesOptions.IsDynamicFeatureStoreEnabled)
        {
            return null;
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetFeatureOrNull(name);
        }
    }

    public virtual async Task<IReadOnlyList<FeatureDefinition>> GetFeaturesAsync()
    {
        if (!FeaturesOptions.IsDynamicFeatureStoreEnabled)
        {
            return Array.Empty<FeatureDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetFeatures().ToImmutableList();
        }
    }

    public virtual async Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync()
    {
        if (!FeaturesOptions.IsDynamicFeatureStoreEnabled)
        {
            return Array.Empty<FeatureGroupDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetGroups().ToImmutableList();
        }
    }

    protected virtual async Task EnsureCacheIsUptoDateAsync()
    {
        if (StoreCache.LastCheckTime.HasValue &&
            DateTime.Now.Subtract(StoreCache.LastCheckTime.Value).TotalSeconds < 30)
        {
            /* We get the latest feature with a small delay for optimization */
            return;
        }

        var stampInDistributedCache = await GetOrSetStampInDistributedCache();

        if (stampInDistributedCache == StoreCache.CacheStamp)
        {
            StoreCache.LastCheckTime = DateTime.Now;
            return;
        }

        await UpdateInMemoryStoreCache();

        StoreCache.CacheStamp = stampInDistributedCache;
        StoreCache.LastCheckTime = DateTime.Now;
    }

    protected virtual async Task UpdateInMemoryStoreCache()
    {
        var featureGroupRecords = await FeatureGroupRepository.GetListAsync();
        var featureRecords = await FeatureRepository.GetListAsync();

        await StoreCache.FillAsync(featureGroupRecords, featureRecords);
    }

    protected virtual async Task<string> GetOrSetStampInDistributedCache()
    {
        var cacheKey = GetCommonStampCacheKey();

        var stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
        if (stampInDistributedCache != null)
        {
            return stampInDistributedCache;
        }

        await using var commonLockHandle = await DistributedLock
            .TryAcquireAsync(GetCommonDistributedLockKey(), TimeSpan.FromMinutes(2));
        if (commonLockHandle == null)
        {
            /* This request will fail */
            throw new AbpException(
                "Could not acquire distributed lock for feature definition common stamp check!"
            );
        }

        stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
        if (stampInDistributedCache != null)
        {
            return stampInDistributedCache;
        }

        stampInDistributedCache = Guid.NewGuid().ToString();

        await DistributedCache.SetStringAsync(
            cacheKey,
            stampInDistributedCache,
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(30) //TODO: Make it configurable?
            }
        );

        return stampInDistributedCache;
    }

    protected virtual string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryFeatureCacheStamp";
    }

    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpFeatureUpdateLock";
    }
}
