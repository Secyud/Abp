using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.AspNetCore.Bundling;
using Secyud.Abp.AspNetCore.Components;
using Secyud.Abp.AspNetCore.Components.Bundling;
using Secyud.Abp.AspNetCore.Localization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Secyud.Abp.AspNetCore;

[DependsOn(
    typeof(AbpAspNetCoreComponentsAppThemingBundlingModule),
    typeof(AbpAspNetCoreComponentsWebSecitsThemeModule)
)]
public class AbpAspNetCoreComponentsAppSecitsThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AppAbpStringLocalizerFactory.Replace(context.Services);
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BlazorSecitsThemeBundles.Styles.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(BlazorAppStandardBundles.Styles.Global)
                        .AddContributors(typeof(BlazorSecitsThemeStyleContributor));
                });

            options
                .ScriptBundles
                .Add(BlazorSecitsThemeBundles.Scripts.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(BlazorAppStandardBundles.Scripts.Global)
                        .AddContributors(typeof(BlazorSecitsThemeScriptContributor));
                });
        });
    }

    public override async Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        if (context.ServiceProvider.GetService<AppPersonalizedProvider>() is { } appPersonalizedProvider)
        {
            await appPersonalizedProvider.SaveAsync();
        }
    
        await base.OnApplicationShutdownAsync(context);
    }
}