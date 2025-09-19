using Secyud.Abp.AspNetCore.Components.Bundling;
using Volo.Abp.AspNetCore.Components.Server;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Secyud.Abp.AspNetCore.Components;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAspNetCoreMvcUiBundlingModule)
)]
public class AbpAspNetCoreComponentsServerThemingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BlazorStandardBundles.Styles.Global, bundle => { bundle.AddContributors(typeof(BlazorGlobalStyleContributor)); });

            options
                .ScriptBundles
                .Add(BlazorStandardBundles.Scripts.Global, bundle => { bundle.AddContributors(typeof(BlazorGlobalScriptContributor)); });
        });
    }
}