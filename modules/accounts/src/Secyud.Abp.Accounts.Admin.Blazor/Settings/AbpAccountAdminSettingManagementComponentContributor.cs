using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Account;
using Secyud.Abp.Account.Localization;
using Secyud.Abp.Accounts.Pages.AccountAdminSettingGroup;
using Secyud.Abp.Settings;

namespace Secyud.Abp.Accounts.Settings;

public class AbpAccountAdminSettingManagementComponentContributor : ISettingComponentContributor
{
    public async Task ConfigureAsync(SettingComponentCreationContext context)
    {
        if (!await CheckPermissionsInternalAsync(context))
        {
            return;
        }

        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpAccountsResource>>();
        context.Groups.Add(
            new SettingComponentGroup(
                "Secyud.Abp.Accounts",
                l["Menu:Accounts"],
                typeof(AccountAdminSettingManagementComponent)
            )
        );
    }

    public virtual async Task<bool> CheckPermissionsAsync(SettingComponentCreationContext context)
    {
        return await CheckPermissionsInternalAsync(context);
    }

    protected virtual async Task<bool> CheckPermissionsInternalAsync(SettingComponentCreationContext context)
    {
        var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();

        return await authorizationService.IsGrantedAsync(AccountPermissions.SettingManagement);
    }
}
