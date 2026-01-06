using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.HttpApi;
using Secyud.Abp.Permissions.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpPermissionsApplicationContractsModule),
    typeof(AbpHttpApiModule)
    )]
public class AbpPermissionsHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpPermissionsHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpPermissionsResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
