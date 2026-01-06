using Microsoft.AspNetCore.Authorization;
using Volo.Abp;

namespace Secyud.Abp.Authorization;

public class PermissionsRequirement : IAuthorizationRequirement
{
    public string[] PermissionNames { get; }
    public bool RequiresAll { get; }

    public PermissionsRequirement(string[] permissionNames, bool requiresAll)
    {
        Check.NotNull(permissionNames, nameof(permissionNames));

        PermissionNames = permissionNames;
        RequiresAll = requiresAll;
    }

    public override string ToString()
    {
        return $"PermissionsRequirement: {string.Join(", ", PermissionNames)}";
    }
}