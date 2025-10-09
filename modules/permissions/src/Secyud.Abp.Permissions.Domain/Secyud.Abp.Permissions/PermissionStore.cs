using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Secyud.Abp.Permissions;

public class PermissionStore(
    IPermissionGrantRepository permissionGrantRepository,
    IDistributedCache<PermissionGrantCacheItem, PermissionGrantCacheKey> cache,
    IPermissionDefinitionManager permissionDefinitionManager)
    : IPermissionStore, ITransientDependency
{
    public ILogger<PermissionStore> Logger { get; set; } = NullLogger<PermissionStore>.Instance;
    protected IPermissionGrantRepository PermissionGrantRepository { get; } = permissionGrantRepository;
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; } = permissionDefinitionManager;
    protected IDistributedCache<PermissionGrantCacheItem, PermissionGrantCacheKey> Cache { get; } = cache;

    public virtual async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
    {
        var items =
            await GetCacheItemsAsync([name], providerName, providerKey);

        return items.FirstOrDefault().Value?.IsGranted ?? false;
    }

    public virtual async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
    {
        Check.NotNullOrEmpty(names, nameof(names));

        var result = new MultiplePermissionGrantResult();

        if (names.Length == 1)
        {
            var name = names.First();
            result.Result.Add(name,
                await IsGrantedAsync(names.First(), providerName, providerKey)
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Undefined);
            return result;
        }

        var cacheItems = await GetCacheItemsAsync(names, providerName, providerKey);
        foreach (var item in cacheItems)
        {
            result.Result.Add(item.Key.Name,
                item.Value is { IsGranted: true }
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Undefined);
        }

        return result;
    }

    protected virtual async Task<KeyValuePair<PermissionGrantCacheKey, PermissionGrantCacheItem?>[]> GetCacheItemsAsync(
        string[] names, string providerName, string providerKey)
    {
        var cacheKeys = names
            .Select(x => new PermissionGrantCacheKey(x, providerName, providerKey)).ToList();

        Logger.LogDebug("PermissionStore.GetCacheItemAsync: {CacheKeys}", string.Join(",", cacheKeys));

        var cacheItems = await Cache.GetManyAsync(cacheKeys);

        if (cacheItems.All(x => x.Value != null))
        {
            Logger.LogDebug("Found in the cache: {Keys}", string.Join(",", cacheKeys));
            return cacheItems;
        }

        var notCacheKeys = cacheItems
            .Where(x => x.Value == null)
            .Select(x => x.Key).ToList();

        Logger.LogDebug("Not found in the cache: {Keys}", string.Join(",", notCacheKeys));

        var newCacheItems = await SetCacheItemsAsync(providerName, providerKey, notCacheKeys);

        var result = new KeyValuePair<PermissionGrantCacheKey, PermissionGrantCacheItem?>[cacheItems.Length];

        for (var i = 0; i < result.Length; i++)
        {
            var (key, value) = cacheItems[i];
            value ??= newCacheItems.FirstOrDefault(u => u.Key == key).Value;
            result[i] = new KeyValuePair<PermissionGrantCacheKey, PermissionGrantCacheItem?>(key, value);
        }

        return result;
    }

    protected virtual async Task<List<KeyValuePair<PermissionGrantCacheKey, PermissionGrantCacheItem>>> SetCacheItemsAsync(
        string providerName, string providerKey, List<PermissionGrantCacheKey> notCacheKeys)
    {
        using (PermissionGrantRepository.DisableTracking())
        {
            var permissions = (await PermissionDefinitionManager.GetPermissionsAsync())
                .Where(x => notCacheKeys.Any(k => k.Name == x.Name)).ToList();

            var permissionNames = permissions.Select(x => x.Name).ToArray();

            Logger.LogDebug("Getting not cache granted permissions from the repository for this provider name,key: {ProviderName},{ProviderKey}",
                providerName, providerKey);

            var grantedPermissions = await PermissionGrantRepository.GetListAsync(permissionNames, providerName, providerKey);

            var grantedPermissionsHashSet = grantedPermissions
                .Select(p => p.Name)
                .ToHashSet();

            Logger.LogDebug("Setting the cache items. Count: {PermissionsCount}", permissions.Count);

            var cacheItems = permissions
                .Select(t => new KeyValuePair<PermissionGrantCacheKey, PermissionGrantCacheItem>(
                    new PermissionGrantCacheKey(t.Name, providerName, providerKey),
                    new PermissionGrantCacheItem(grantedPermissionsHashSet.Contains(t.Name)))).ToList();

            await Cache.SetManyAsync(cacheItems, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(7)
            });

            Logger.LogDebug("Finished setting the cache items. Count: {PermissionsCount}", permissions.Count);

            return cacheItems;
        }
    }
}