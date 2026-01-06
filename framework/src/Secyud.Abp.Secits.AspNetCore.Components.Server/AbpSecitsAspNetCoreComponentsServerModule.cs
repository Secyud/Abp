using Secyud.Abp.Secits.AspNetCore.Components.Bundling;
using Volo.Abp.AspNetCore.Components.Server;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Secits.AspNetCore.Components;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerModule),
    typeof(AbpSecitsAspNetCoreComponentsWebModule)
)]
public class AbpSecitsAspNetCoreComponentsServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BlazorServerBundles.Styles.Global,
                    bundle => { bundle.AddContributors(typeof(BlazorServerStyleContributor)); });

            options
                .ScriptBundles
                .Add(BlazorServerBundles.Scripts.Global,
                    bundle => { bundle.AddContributors(typeof(BlazorServerScriptContributor)); });
        });
    }
}