using System.Security.Claims;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionValuesCheckContext(
    List<PermissionDefinition> permissions,
    ClaimsPrincipal? principal)
{
    public List<PermissionDefinition> Permissions { get; } = permissions;

    public ClaimsPrincipal? Principal { get; } = principal;
}