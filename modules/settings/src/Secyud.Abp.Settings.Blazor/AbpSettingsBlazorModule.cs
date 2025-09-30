using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.AspNetCore.Components;
using Secyud.Abp.AspNetCore.Components.Routing;
using Secyud.Abp.Settings.Localization;
using Secyud.Abp.Settings.Menus;
using Secyud.Abp.Settings.Settings;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.Settings;

[DependsOn(
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpSettingsApplicationContractsModule)
)]
public class AbpSettingsBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpSettingsBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<SettingsBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new SettingsMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpSettingsBlazorModule).Assembly);
        });

        Configure<SettingsComponentOptions>(options =>
        {
            options.Contributors.Add(new EmailingPageContributor());
            options.Contributors.Add(new TimeZonePageContributor());
        });
        
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
