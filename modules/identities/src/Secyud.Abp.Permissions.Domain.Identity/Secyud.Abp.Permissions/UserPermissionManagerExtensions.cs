using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;

namespace Secyud.Abp.Permissions;

public static class UserPermissionManagerExtensions
{
    public static Task<List<PermissionGrantInfo>> GetAllForUserAsync(this IPermissionManager permissionManager, Guid userId)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.GetListAsync(null, UserPermissionValueProvider.ProviderName, userId.ToString());
    }

    public static Task SetForUserAsync(this IPermissionManager permissionManager, Guid userId, string name, bool isGranted)
    {
        string[] permissions = [name];
        return permissionManager.UpdateAsync(UserPermissionValueProvider.ProviderName, userId.ToString(),
            isGranted ? permissions : [], isGranted ? [] : permissions);
    }
}