﻿using Localization.Resources.AbpUi;
using SecitsDemoApp.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class SecitsDemoAppHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(SecitsDemoAppHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<SecitsDemoAppResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
