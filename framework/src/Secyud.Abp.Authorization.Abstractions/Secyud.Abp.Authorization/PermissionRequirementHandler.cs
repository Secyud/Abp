using Microsoft.AspNetCore.Authorization;
using Secyud.Abp.Authorization.Permissions;

namespace Secyud.Abp.Authorization;

public class PermissionRequirementHandler(IPermissionChecker permissionChecker)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (await permissionChecker.IsGrantedAsync(context.User, requirement.PermissionName) ==
            PermissionGrantResult.Granted)
        {
            context.Succeed(requirement);
        }
    }
}