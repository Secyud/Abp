using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Features.Components.FeatureSettingGroup;
using Secyud.Abp.Features.Localization;
using Secyud.Abp.Settings;

namespace Secyud.Abp.Features.Settings;

public class FeatureSettingsComponentContributor: ISettingComponentContributor
{
    public virtual async Task ConfigureAsync(SettingComponentCreationContext context)
    {
        if (!await CheckPermissionsInternalAsync(context))
        {
            return;
        }

        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpFeaturesResource>>();
        context.Groups.Add(
            new SettingComponentGroup(
                "Secyud.Abp.Feature",
                l["Permission:Features"],
                typeof(FeatureSettingsComponent)
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

        return await authorizationService.IsGrantedAsync(FeaturesPermissions.ManageHostFeatures);
    }
}
