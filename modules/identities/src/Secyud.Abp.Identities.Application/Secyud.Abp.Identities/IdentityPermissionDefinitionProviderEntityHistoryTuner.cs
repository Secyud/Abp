using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization.Permissions;

namespace Secyud.Abp.Identities;

public class IdentityPermissionDefinitionProviderEntityHistoryTuner : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var auditHelper = context.ServiceProvider.GetRequiredService<IAuditingHelper>();

        if (!auditHelper.IsEntityHistoryEnabled(typeof(IdentityUser)))
        {
            context.TryDisablePermission(IdentityPermissions.Users.ViewChangeHistory.Name);
        }

        if (!auditHelper.IsEntityHistoryEnabled(typeof(IdentityRole)))
        {
            context.TryDisablePermission(IdentityPermissions.Roles.ViewChangeHistory.Name);
        }
    }
}