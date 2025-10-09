using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace Secyud.Abp.Permissions;

[UnitOfWork]
public class PermissionManager(
    IPermissionDefinitionRecordRepository permissionRepository,
    IPermissionGrantRepository grantRepository,
    IPermissionGroupDefinitionRecordRepository groupRepository,
    IUnitOfWorkManager uowManager,
    IOptions<AbpPermissionOptions> options,
    IDistributedCache<PermissionGrantCacheItem, PermissionGrantCacheKey> cache,
    IServiceProvider serviceProvider) : DomainService, IPermissionManager
{
    public IDistributedCache<PermissionGrantCacheItem, PermissionGrantCacheKey> Cache { get; } = cache;
    protected AbpPermissionOptions Options { get; } = options.Value;
    protected List<string>? TotalProviders { get; set; }


    public virtual async Task<PermissionGrantInfo> GetAsync(string name, string providerName, string providerKey)
    {
        var query = await GetPermissionWithGrantQueryableAsync(providerName, providerKey,
            q => q
                .Where(p => p.Name == name)
        );

        var permission = query.FirstOrDefault();

        if (permission is null)
        {
            return new PermissionGrantInfo(name, false)
            {
                Providers = []
            };
        }

        return MapToPermissionGrantInfo(permission);
    }

    public async Task<List<PermissionGrantInfo>> GetAsync(string[] names, string providerName, string providerKey)
    {
        var query = await GetPermissionWithGrantQueryableAsync(providerName, providerKey,
            q => q
                .Where(p => names.Contains(p.Name))
        );
        var result = query.AsEnumerable().Select(MapToPermissionGrantInfo).ToList();

        return result;
    }

    public virtual async Task<List<PermissionGrantInfo>> GetListAsync(string? groupName, string providerName, string providerKey)
    {
        var query = await GetPermissionWithGrantQueryableAsync(providerName, providerKey,
            q => q
                .WhereIf(!groupName.IsNullOrEmpty(), p => p.GroupName == groupName)
        );

        var result = query.AsEnumerable().Select(MapToPermissionGrantInfo).ToList();

        return result;
    }

    public virtual async Task<List<PermissionGroupInfo>> GetGroupsAsync()
    {
        var groupQuery = await groupRepository.GetQueryableAsync();

        var result = groupQuery.Select(u => new PermissionGroupInfo(u.Name)
        {
            DisplayName = u.DisplayName
        }).ToList();

        return result;
    }

    public virtual async Task UpdateAsync(string providerName, string providerKey, string[] grantedPermissions, string[] deniedPermissions)
    {
        CheckIfProviderExist(providerName);

        var allPermissions = grantedPermissions.Union(deniedPermissions);

        var query = await GetPermissionWithGrantQueryableAsync(providerName, providerKey,
            q => q
                .Where(p => allPermissions.Contains(p.Name))
        );

        var grantedHashSet = grantedPermissions.ToHashSet();
        var deniedHashSet = deniedPermissions.ToHashSet();

        var deleteGrants = new List<PermissionGrant>();
        var insertGrants = new List<PermissionGrant>();
        var totalGrants = new List<PermissionGrant>();

        var permissions = query.ToList();

        foreach (var info in permissions)
        {
            var (permission, grant) = (info.Permission, info.Grant);

            if (deniedHashSet.Contains(permission.Name) && grant is not null)
            {
                deleteGrants.Add(grant);
            }

            if (grantedHashSet.Contains(permission.Name) && grant is null)
            {
                grant = new PermissionGrant(
                    GuidGenerator.Create(), permission.Name, providerName, providerKey);
                insertGrants.Add(grant);
            }

            if (grant is not null)
                totalGrants.Add(grant);
        }

        await grantRepository.InsertManyAsync(insertGrants);
        await grantRepository.DeleteManyAsync(deleteGrants);

        if (uowManager.Current is not null)
            await uowManager.Current.SaveChangesAsync();

        await ClearCacheAsync(totalGrants);
    }

    public virtual async Task DeleteAsync(string providerName, string providerKey)
    {
        CheckIfProviderExist(providerName);
        await grantRepository.DeleteDirectAsync(u
            => u.ProviderKey == providerKey && u.ProviderName == providerName);
    }

    public async Task ClearCacheAsync(List<PermissionGrant> grants)
    {
        var cacheKeys = grants.Select(u =>
            new PermissionGrantCacheKey(u.Name, u.ProviderName, u.ProviderKey));
        await Cache.RemoveManyAsync(cacheKeys, true);
    }

    protected void CheckIfProviderExist(string providerName)
    {
        var providers = GetTotalProviders();
        if (!providers.Contains(providerName))
        {
            throw new BusinessException(AbpPermissionsErrorCodes.PermissionValueProviderNotFound,
                    $"Unknown permission value provider: {providerName}")
                .WithData("value", providerName);
        }
    }

    protected List<string> GetTotalProviders()
    {
        TotalProviders ??= Options.ValueProviders
            .Select(serviceProvider.GetService)
            .Cast<IPermissionValueProvider?>()
            .Where(u => u is not null)
            .Select(u => u!.Name).ToList();

        return TotalProviders;
    }


    protected virtual List<string> DeserializeProviders(string? provider)
    {
        if (!provider.IsNullOrEmpty())
        {
            return provider.Split(',').ToList();
        }

        return GetTotalProviders();
    }

    private PermissionGrantInfo MapToPermissionGrantInfo(PermissionWithGrant pwg)
    {
        var permission = pwg.Permission;
        return new PermissionGrantInfo(pwg.Permission.Name, pwg.Grant is not null)
        {
            IsEnabled = permission.IsEnabled,
            GroupName = permission.GroupName,
            DisplayName = permission.DisplayName,
            ParentName = permission.ParentName,
            Providers = DeserializeProviders(permission.Providers)
        };
    }

    private async Task<IQueryable<PermissionWithGrant>> GetPermissionWithGrantQueryableAsync(string providerName, string providerKey,
        Func<IQueryable<PermissionDefinitionRecord>, IQueryable<PermissionDefinitionRecord>>? queryFunc)
    {
        var permissionQuery = await permissionRepository.GetQueryableAsync();
        var grantQuery = await grantRepository.GetQueryableAsync();

        permissionQuery = permissionQuery
                .Where(p => p.IsDynamic)
            ;

        if (queryFunc is not null)
            permissionQuery = queryFunc(permissionQuery);

        var query = from permission in permissionQuery
            join g in grantQuery on
                new { permission.Name, ProviderName = providerName, ProviderKey = providerKey, TenantId = CurrentTenant.Id } equals
                new { g.Name, g.ProviderName, g.ProviderKey, g.TenantId } into grantGroup
            from grant in grantGroup.DefaultIfEmpty()
            select new PermissionWithGrant(permission, grant);

        return query;
    }

    private class PermissionWithGrant(PermissionDefinitionRecord permission, PermissionGrant? grant)
    {
        public PermissionDefinitionRecord Permission { get; set; } = permission;
        public PermissionGrant? Grant { get; set; } = grant;
    }
}