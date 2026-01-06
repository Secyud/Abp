using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Secyud.Abp.Authorization.Permissions;
using Volo.Abp;
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

    public virtual async Task<PermissionGrantResult> IsGrantedAsync(
        string name, string providerName, string providerKey)
    {
        if (await CheckPermissionUndefinedAsync(name, providerName))
            return PermissionGrantResult.Undefined;

        var res = await GetCacheItemAsync(name, providerName, providerKey);

        return res.IsGranted ? PermissionGrantResult.Granted : PermissionGrantResult.Unset;
    }

    protected virtual async Task<PermissionGrantCacheItem> GetCacheItemAsync(
        string name, string providerName, string providerKey)
    {
        var key = new PermissionGrantCacheKey(name, providerName, providerKey);
        var cacheItem = await Cache.GetOrAddAsync(key, () => GetStoreItemAsync(name, providerName, providerKey));

        if (cacheItem is null)
        {
            throw new BusinessException(AbpPermissionsErrorCodes.PermissionCacheItemSetFailed,
                    "Permission grant cache set failed!")
                .WithData("name", name)
                .WithData("providerName", providerName)
                .WithData("providerKey", providerKey);
        }

        return cacheItem;
    }

    protected virtual async Task<PermissionGrantCacheItem> GetStoreItemAsync(
        string name, string providerName, string providerKey)
    {
        using var tracking = PermissionGrantRepository.DisableTracking();
        var grant = await PermissionGrantRepository.FindAsync(name, providerName, providerKey);
        var res = new PermissionGrantCacheItem
        {
            IsGranted = grant is not null
        };
        return res;
    }


    public virtual async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName,
        string providerKey)
    {
        Check.NotNullOrEmpty(names, nameof(names));

        var res = new MultiplePermissionGrantResult();
        var definedPermissions = new List<string>();
        foreach (var name in names)
        {
            res.Result[name] = PermissionGrantResult.Undefined;
            if (!await CheckPermissionUndefinedAsync(name, providerName))
            {
                definedPermissions.Add(name);
            }
        }


        var cacheItems = await GetCacheItemsAsync(
            definedPermissions.ToArray(), providerName, providerKey);
        foreach (var item in cacheItems)
        {
            res.Result.Add(item.Key.Name,
                item.Value is { IsGranted: true }
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Unset);
        }

        return res;
    }

    protected virtual async Task<Dictionary<PermissionGrantCacheKey, PermissionGrantCacheItem>> GetCacheItemsAsync(
        string[] names, string providerName, string providerKey)
    {
        var cacheKeys = names
            .Select(x => new PermissionGrantCacheKey(x, providerName, providerKey)).ToList();

        var cacheItems = await Cache.GetManyAsync(cacheKeys);

        if (cacheItems.All(x => x.Value != null))
        {
            return cacheItems.ToDictionary(
                u => u.Key,
                u => u.Value!);
        }

        var notCacheKeys = cacheItems
            .Where(x => x.Value == null)
            .Select(x => x.Key.Name).ToArray();

        var newCacheItems = await SetCacheItemsAsync(providerName, providerKey, notCacheKeys);

        var res = new Dictionary<PermissionGrantCacheKey, PermissionGrantCacheItem>();

        foreach (var (key, value) in cacheItems)
        {
            res[key] = value ?? newCacheItems[key];
        }

        return res;
    }

    protected virtual async Task<Dictionary<PermissionGrantCacheKey, PermissionGrantCacheItem>>
        SetCacheItemsAsync(string providerName, string providerKey, string[] notCacheKeys)
    {
        using var tracking = PermissionGrantRepository.DisableTracking();

        var grantedPermissions =
            await PermissionGrantRepository.GetListAsync(notCacheKeys, providerName, providerKey);

        var grantedPermissionsHashSet = grantedPermissions
            .Select(p => p.Name)
            .ToHashSet();

        var cacheItems = notCacheKeys
            .ToDictionary(
                t => new PermissionGrantCacheKey(t, providerName, providerKey),
                t => new PermissionGrantCacheItem(grantedPermissionsHashSet.Contains(t)));

        await Cache.SetManyAsync(cacheItems, new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromDays(7)
        });

        return cacheItems;
    }

    protected async Task<bool> CheckPermissionUndefinedAsync(string name, string providerName)
    {
        var permission = await PermissionDefinitionManager.GetOrNullAsync(name);
        return permission is null || permission.Providers.Count > 0 && !permission.Providers.Contains(providerName);
    }
}