using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionDefinitionManager(PermissionDefinitionStore store)
    : IPermissionDefinitionManager, ITransientDependency
{
    protected PermissionDefinitionStore Store { get; } = store;


    public virtual async Task<PermissionDefinition> GetAsync(string name)
    {
        var permission = await GetOrNullAsync(name);
        
        return permission ?? throw new AbpException("Undefined permission: " + name);
    }

    public virtual async Task<PermissionDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));
        
        return await Store.GetOrNullAsync(name);
    }

    public virtual async Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        return await Store.GetPermissionsAsync();
    }

    public virtual async Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        return await Store.GetGroupsAsync();
    }
}