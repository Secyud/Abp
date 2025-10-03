using Localization.Resources.AbpUi;
using Secyud.Abp.AspNetCore.Components;
using Secyud.Abp.Features.Localization;
using Secyud.Abp.Features.Settings;
using Secyud.Abp.Settings;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpFeaturesApplicationContractsModule),
    typeof(AbpFeaturesModule),
    typeof(AbpSettingsBlazorModule)
)]
public class AbpFeaturesBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<SettingsComponentOptions>(options =>
        {
            options.Contributors.Add(new FeatureSettingsComponentContributor());
        });
        
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpFeaturesResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
