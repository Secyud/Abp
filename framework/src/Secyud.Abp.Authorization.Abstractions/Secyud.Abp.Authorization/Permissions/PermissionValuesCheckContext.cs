using System.Security.Claims;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionValuesCheckContext(
    List<IPermissionDefinition> permissions,
    ClaimsPrincipal? principal)
{
    public List<IPermissionDefinition> Permissions { get; } = permissions;

    public ClaimsPrincipal? Principal { get; } = principal;
}