using System.Collections.Immutable;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Settings;
using Volo.Abp.Threading;

namespace Secyud.Abp.Settings;

[Dependency(ReplaceServices = true)]
public class DynamicSettingDefinitionStore(
    ISettingDefinitionRecordRepository textSettingRepository,
    ISettingDefinitionSerializer textSettingDefinitionSerializer,
    IDynamicSettingDefinitionStoreInMemoryCache storeCache,
    IDistributedCache distributedCache,
    IOptions<AbpDistributedCacheOptions> cacheOptions,
    IOptions<SettingsOptions> SettingsOptions,
    IAbpDistributedLock distributedLock)
    : IDynamicSettingDefinitionStore, ITransientDependency
{
    protected ISettingDefinitionRecordRepository SettingRepository { get; } = textSettingRepository;
    protected ISettingDefinitionSerializer SettingDefinitionSerializer { get; } = textSettingDefinitionSerializer;
    protected IDynamicSettingDefinitionStoreInMemoryCache StoreCache { get; } = storeCache;
    protected IDistributedCache DistributedCache { get; } = distributedCache;
    protected IAbpDistributedLock DistributedLock { get; } = distributedLock;
    public SettingsOptions SettingsOptions { get; } = SettingsOptions.Value;
    protected AbpDistributedCacheOptions CacheOptions { get; } = cacheOptions.Value;

    public virtual async Task<SettingDefinition> GetAsync(string name)
    {
        var setting = await GetOrNullAsync(name);
        if (setting == null)
        {
            throw new AbpException("Undefined setting: " + name);
        }

        return setting;
    }

    public virtual async Task<SettingDefinition?> GetOrNullAsync(string name)
    {
        if (!SettingsOptions.IsDynamicSettingStoreEnabled)
        {
            return null;
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetSettingOrNull(name);
        }
    }

    public virtual async Task<IReadOnlyList<SettingDefinition>> GetAllAsync()
    {
        if (!SettingsOptions.IsDynamicSettingStoreEnabled)
        {
            return Array.Empty<SettingDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetSettings().ToImmutableList();
        }
    }

    protected virtual async Task EnsureCacheIsUptoDateAsync()
    {
        if (StoreCache.LastCheckTime.HasValue &&
            DateTime.Now.Subtract(StoreCache.LastCheckTime.Value).TotalSeconds < 30)
        {
            /* We get the latest setting with a small delay for optimization */
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
        var settingRecords = await SettingRepository.GetListAsync();
        await StoreCache.FillAsync(settingRecords);
    }

    protected virtual async Task<string> GetOrSetStampInDistributedCache()
    {
        var cacheKey = GetCommonStampCacheKey();

        var stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
        if (stampInDistributedCache != null)
        {
            return stampInDistributedCache;
        }

        await using var commonLockHandle = await DistributedLock.TryAcquireAsync(GetCommonDistributedLockKey(), TimeSpan.FromMinutes(2));
        if (commonLockHandle == null)
        {
            /* This request will fail */
            throw new AbpException(
                "Could not acquire distributed lock for setting definition common stamp check!"
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
        return $"{CacheOptions.KeyPrefix}_AbpInMemorySettingCacheStamp";
    }

    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpSettingUpdateLock";
    }
}
