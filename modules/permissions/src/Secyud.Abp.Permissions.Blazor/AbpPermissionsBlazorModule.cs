using Localization.Resources.AbpUi;
using Secyud.Abp.Permissions.Localization;
using Secyud.Abp.Secits.AspNetCore.Components;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpSecitsAspNetCoreComponentsWebModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpPermissionsApplicationContractsModule)
)]
public class AbpPermissionsBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options
            =>
        {
            options.AddProfile<AbpPermissionsBlazorAutoMapperProfile>();
        });

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