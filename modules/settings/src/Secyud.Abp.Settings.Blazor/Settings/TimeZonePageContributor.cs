using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Settings.Components.TimeZoneSettingGroup;
using Secyud.Abp.Settings.Localization;
using Volo.Abp.Timing;

namespace Secyud.Abp.Settings.Settings;

public class TimeZonePageContributor : ISettingComponentContributor
{
    public async Task ConfigureAsync(SettingComponentCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpSettingsResource>>();
        if (await CheckPermissionsAsync(context))
        {
            context.Groups.Add(
                new SettingComponentGroup(
                    "Volo.Abp.TimeZone",
                    l["Menu:TimeZone"],
                    typeof(TimeZoneSettingGroupViewComponent)
                )
            );
        }
    }

    public async Task<bool> CheckPermissionsAsync(SettingComponentCreationContext context)
    {
        var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();

        return context.ServiceProvider.GetRequiredService<IClock>().SupportsMultipleTimezone
            && await authorizationService.IsGrantedAsync(SettingsPermissions.TimeZone);
    }
}
