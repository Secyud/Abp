using Volo.Abp.Authorization.Permissions;

namespace Secyud.Abp.Permissions;

public static class RolePermissionManagerExtensions
{
    public static Task<PermissionGrantInfo> GetForRoleAsync(this IPermissionManager permissionManager, string roleName, string permissionName)
    {
        return permissionManager.GetAsync(permissionName, RolePermissionValueProvider.ProviderName, roleName);
    }

    public static Task<List<PermissionGrantInfo>> GetAllForRoleAsync(this IPermissionManager permissionManager, string roleName)
    {
        return permissionManager.GetListAsync(null, RolePermissionValueProvider.ProviderName, roleName);
    }

    public static Task SetForRoleAsync(this IPermissionManager permissionManager, string roleName, string permissionName, bool isGranted)
    {
        string[] permissions = [permissionName];
        return permissionManager.UpdateAsync(RolePermissionValueProvider.ProviderName, roleName,
            isGranted ? permissions : [], isGranted ? [] : permissions);
    }
}