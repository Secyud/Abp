using Volo.Abp.Authorization.Permissions;

namespace Secyud.Abp.Permissions;

public class TestPermissionValueProvider(IPermissionStore permissionStore) : PermissionValueProvider(permissionStore)
{
    public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        if (await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, "Test"))
            return PermissionGrantResult.Granted;
        return PermissionGrantResult.Undefined;
    }

    public override async Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        var result = await PermissionStore
            .IsGrantedAsync(context.Permissions.Select(u => u.Name).ToArray(), Name, "Test");
        
        return result;
    }

    public override string Name => "Test";
}
