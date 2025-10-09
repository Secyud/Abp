namespace Volo.Abp.Authorization.Permissions;

public static class AuthorizationPermissionsExtensions
{
    public static PermissionGroupDefinition AddGroup(this IPermissionDefinitionContext context, IPermission permission)
    {
        return context.AddGroup(permission.Name, permission.LocalizableString);
    }

    public static PermissionDefinition AddPermission(this PermissionGroupDefinition group, IPermission permission)
    {
        return group.AddPermission(permission.Name, permission.LocalizableString, permission.MultiTenancySides, permission.IsEnabled);
    }

    public static PermissionDefinition AddChild(this PermissionDefinition group, IPermission permission)
    {
        return group.AddChild(permission.Name, permission.LocalizableString, permission.MultiTenancySides, permission.IsEnabled);
    }
}