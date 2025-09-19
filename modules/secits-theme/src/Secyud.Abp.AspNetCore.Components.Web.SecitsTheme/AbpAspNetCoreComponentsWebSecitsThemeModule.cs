using Secyud.Abp.AspNetCore.Components;
using Secyud.Abp.AspNetCore.Components.Routing;
using Secyud.Abp.AspNetCore.Components.Theming;
using Secyud.Abp.AspNetCore.Components.Toolbars;
using Secyud.Abp.AspNetCore.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.AspNetCore;

[DependsOn(
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule)
)]
public class AbpAspNetCoreComponentsWebSecitsThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
        ConfigureSecitsStyles();
        ConfigureSecitsTheme();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets
                .AddEmbedded<AbpAspNetCoreComponentsWebSecitsThemeModule>();
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
            o.Styles[SecitsStyleNames.Light] =
                new SecitsThemeStyle(L("Theme:" + SecitsStyleNames.Light), "theme-icon-light");

            o.Styles[SecitsStyleNames.Default] =
                new SecitsThemeStyle(L("Theme:" + SecitsStyleNames.Default), "theme-icon-default");

            o.Styles[SecitsStyleNames.Dark] =
                new SecitsThemeStyle(L("Theme:" + SecitsStyleNames.Dark), "theme-icon-dark");

            o.Styles[SecitsStyleNames.System] =
                new SecitsThemeStyle(L("Theme:" + SecitsStyleNames.System), "theme-icon-system");
        });
    }

    private void ConfigureSecitsTheme()
    {
        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<SecitsTheme>();
            options.DefaultThemeName ??= SecitsTheme.Name;
        });
    }

    private static LocalizableString L(string key)
    {
        return LocalizableString.Create<SecitsResource>(key);
    }
}