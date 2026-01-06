using Secyud.Abp.Secits.AspNetCore.Components.Localization;
using Secyud.Abp.Secits.AspNetCore.Components.Theming;
using Secyud.Abp.Secits.AspNetCore.Components.Toolbars;
using Secyud.Abp.Secits.Blazor;
using Secyud.Abp.Ui.Navigation;
using Secyud.Secits.Blazor.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Secits.AspNetCore.Components;

[DependsOn(
    typeof(AbpAspNetCoreBundlingModule),
    typeof(AbpSecitsBlazorModule),
    typeof(AbpUiNavigationModule)
)]
public class AbpSecitsAspNetCoreComponentsWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDynamicLayoutComponentOptions>(options =>
        {
            options.Components.Add(typeof(AbpAuthenticationState), null);
        });
        ConfigureLocalization();
        ConfigureSecitsStyles();
        ConfigureToolbarOptions();
    }

    private void ConfigureToolbarOptions()
    {
        Configure<AbpToolbarOptions>(options
            =>
        {
            options.Contributors.Add(new DefaultToolbarContributor());
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets
                .AddEmbedded<AbpSecitsAspNetCoreComponentsWebModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<SecitsResource>("en")
                .AddVirtualJson("/Localization/Secits");
        });
    }

    private void ConfigureSecitsStyles()
    {
        Configure<SecitsThemeOptions>(o =>
        {
            o.Styles[SecitsThemeNames.System] =
                new SecitsThemeStyle(L("Theme:" + SecitsThemeNames.System), "theme-icon-system")
                {
                    Order = -2
                };

            o.Styles[SecitsThemeNames.Light] =
                new SecitsThemeStyle(L("Theme:" + SecitsThemeNames.Light), "theme-icon-light")
                {
                    Order = -1
                };

            o.Styles[SecitsThemeNames.Dark] =
                new SecitsThemeStyle(L("Theme:" + SecitsThemeNames.Dark), "theme-icon-dark")
                {
                    Order = -1,
                    Parameters =
                    {
                        [SecitsStylesOptions.Color] = "dark"
                    }
                };
        });
    }


    private static LocalizableString L(string key)
    {
        return LocalizableString.Create<SecitsResource>(key);
    }
}