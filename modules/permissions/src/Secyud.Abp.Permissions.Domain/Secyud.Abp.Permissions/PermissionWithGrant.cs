namespace Secyud.Abp.Permissions;

public class PermissionWithGrant(PermissionDefinitionRecord permission, PermissionGrant? grant)
{
    public PermissionDefinitionRecord Permission { get; set; } = permission;
    public PermissionGrant? Grant { get; set; } = grant;
}