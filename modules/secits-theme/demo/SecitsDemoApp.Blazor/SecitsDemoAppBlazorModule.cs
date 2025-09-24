﻿using Microsoft.Extensions.DependencyInjection;
using SecitsDemoApp.Menus;
using Secyud.Abp.AspNetCore;
using Secyud.Abp.AspNetCore.Components.Routing;
using Secyud.Abp.AspNetCore.Styles;
using Secyud.Secits.Blazor;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppApplicationContractsModule),
    typeof(AbpAspNetCoreComponentsServerSecitsThemeModule),
    typeof(AbpAutoMapperModule)
)]
public class SecitsDemoAppBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<SecitsDemoAppBlazorModule>();
        context.Services.AddSecitsFontAwesome();

        context.Services.AddAlwaysAllowAuthorization();
        // Configure<SecitsThemeOptions>(options =>
        // {
        //     options.UseApplicationTabs = false;
        // });
        Configure<AbpAutoMapperOptions>(options
            =>
        {
            options.AddProfile<SecitsDemoAppBlazorAutoMapperProfile>();
        });

        Configure<AbpNavigationOptions>(options
            =>
        {
            options.MenuContributors.Add(new SecitsDemoAppMenuContributor());
        });

        Configure<AbpRouterOptions>(options
            =>
        {
            options.AdditionalAssemblies.Add(typeof(SecitsDemoAppBlazorModule).Assembly);
        });
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
        });
    }
}