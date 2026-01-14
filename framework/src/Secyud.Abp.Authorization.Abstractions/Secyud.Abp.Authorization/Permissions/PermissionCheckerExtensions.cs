using System.Security;

namespace Secyud.Abp.Authorization.Permissions;

public static class PermissionCheckerExtensions
{
    extension(IPermissionChecker checker)
    {
        public async Task<bool> HasPermissionAsync(string permissionName)
        {
            var result = await checker.IsGrantedAsync(permissionName);
            return result == PermissionGrantResult.Granted;
        }
    }
}