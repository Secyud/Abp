using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Features;
using Secyud.Abp.Tenants.Localization;
using Secyud.Abp.Features.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Tenants;

[DependsOn(
    typeof(AbpTenantsApplicationContractsModule),
    typeof(AbpFeaturesHttpApiModule),
    typeof(AbpAspNetCoreMvcModule)
    )]
public class AbpTenantsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpTenantsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpTenantsResource>()
                .AddBaseTypes(
                    typeof(AbpFeaturesResource),
                    typeof(AbpUiResource));
        });
    }
}
