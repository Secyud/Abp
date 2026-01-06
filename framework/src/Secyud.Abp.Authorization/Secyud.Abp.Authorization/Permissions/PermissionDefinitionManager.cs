using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionDefinitionManager(IPermissionDefinitionStore store)
    : IPermissionDefinitionManager, ITransientDependency
{
    protected IPermissionDefinitionStore Store { get; } = store;


    public virtual async Task<IPermissionDefinition> GetAsync(string name)
    {
        var permission = await GetOrNullAsync(name);
        
        return permission ?? throw new AbpException("Undefined permission: " + name);
    }

    public virtual async Task<IPermissionDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));
        
        return await Store.GetOrNullAsync(name);
    }

    public virtual async Task<IReadOnlyList<IPermissionDefinition>> GetPermissionsAsync()
    {
        return await Store.GetPermissionsAsync();
    }

    public virtual async Task<IReadOnlyList<IPermissionDefinition>> GetGroupsAsync()
    {
        return await Store.GetGroupsAsync();
    }
}