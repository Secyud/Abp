using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Authorization.Permissions;

public interface IWithPermissions
{
    PermissionDefinition AddPermission(
        string permissionName,
        ILocalizableString? displayName = null,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both);

    bool RemovePermission(string permissionName, bool recurse = false);
}