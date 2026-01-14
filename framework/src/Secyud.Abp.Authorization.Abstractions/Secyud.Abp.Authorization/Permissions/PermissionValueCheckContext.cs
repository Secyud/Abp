using System.Security.Claims;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionValueCheckContext(
    PermissionDefinition permission,
    ClaimsPrincipal? principal)
{
    public PermissionDefinition Permission { get; } = permission;

    public ClaimsPrincipal? Principal { get; } = principal;
}
