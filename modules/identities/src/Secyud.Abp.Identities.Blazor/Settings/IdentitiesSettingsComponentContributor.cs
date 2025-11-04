using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Identities.IdentitySettingGroup;
using Secyud.Abp.Identities.Localization;
using Secyud.Abp.Identities.Navigation;
using Secyud.Abp.Settings;

namespace Secyud.Abp.Identities.Settings;

public class IdentitiesSettingsComponentContributor : ISettingComponentContributor
{
    public virtual async Task ConfigureAsync(SettingComponentCreationContext context)
    {
        if (!await CheckPermissionsInternalAsync(context))
        {
            return;
        }

        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpIdentitiesResource>>();
        context.Groups.Add(
            new SettingComponentGroup(
                "Secyud.Abp.Identities",
                l[IdentitiesMenus.Settings.DisplayName],
                typeof(IdentitySettingManagementComponent)
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

        return await authorizationService.IsGrantedAsync(IdentityPermissions.Settings.Default.Name);
    }
}