using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Settings.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Settings;

[DependsOn(
    typeof(AbpSettingsApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpSettingsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpSettingsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpSettingsResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
