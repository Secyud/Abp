using Secyud.Abp.Authorization.Permissions;

namespace Secyud.Abp.Permissions;

public class TestPermissionValueProvider(IPermissionStore permissionStore) : IPermissionGrantProvider
{
    public string Name => "Test";
    
    public async Task<PermissionGrantResult> CheckAsync(PermissionGrantCheckContext context)
    {
        return await permissionStore.IsGrantedAsync(context.Permission.Name, Name, "Test");
    }

    public async Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        var result = await permissionStore
            .IsGrantedAsync(context.Permissions.Select(u => u.Name).ToArray(), Name, "Test");
        
        return result;
    }
}
