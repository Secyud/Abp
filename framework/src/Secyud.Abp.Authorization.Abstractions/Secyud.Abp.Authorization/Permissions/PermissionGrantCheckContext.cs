using System.Security.Claims;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionGrantCheckContext(
    IPermissionDefinition permission,
    ClaimsPrincipal? principal)
{
    public IPermissionDefinition Permission { get; } = permission;

    public ClaimsPrincipal? Principal { get; } = principal;
}
