using Secyud.Abp.Secits.AspNetCore.Components.Bundling;
using Secyud.Abp.Secits.AspNetCore.Components.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Secits.AspNetCore.Components;

[DependsOn(
    typeof(AbpSecitsAspNetCoreComponentsWebModule)
)]
public class AbpSecitsAspNetCoreComponentsWebViewModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        WebViewAbpStringLocalizerFactory.Replace(context.Services);
        
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BlazorWebViewBundles.Styles.Global,
                    bundle => { bundle.AddContributors(typeof(BlazorWebViewStyleContributor)); });

            options
                .ScriptBundles
                .Add(BlazorWebViewBundles.Scripts.Global,
                    bundle => { bundle.AddContributors(typeof(BlazorWebViewScriptContributor)); });
        });
    }
}