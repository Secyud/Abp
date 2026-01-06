using Microsoft.AspNetCore.Authorization;
using Secyud.Abp.Authorization.Permissions;

namespace Secyud.Abp.Authorization;

public class PermissionsRequirementHandler(IPermissionChecker permissionChecker)
    : AuthorizationHandler<PermissionsRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionsRequirement requirement)
    {
        var multiplePermissionGrantResult =
            await permissionChecker.IsGrantedAsync(context.User, requirement.PermissionNames);

        if (requirement.RequiresAll
                ? multiplePermissionGrantResult.AllGranted
                : multiplePermissionGrantResult.Result.Any(x => x.Value == PermissionGrantResult.Granted))
        {
            context.Succeed(requirement);
        }
    }
}