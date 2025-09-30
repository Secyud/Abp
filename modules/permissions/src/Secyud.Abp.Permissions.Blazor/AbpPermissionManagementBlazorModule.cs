using Localization.Resources.AbpUi;
using Secyud.Abp.AspNetCore.Components;
using Secyud.Abp.Permissions.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
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