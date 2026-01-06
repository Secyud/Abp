using Secyud.Abp.Secits.AspNetCore.Components.Bundling;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Secits.AspNetCore.Components;

[DependsOn(
    typeof(AbpSecitsAspNetCoreComponentsWebModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyModule)
)]
public class AbpSecitsAspNetCoreComponentsWebAssemblyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BlazorWebAssemblyBundles.Styles.Global,
                    bundle => { bundle.AddContributors(typeof(BlazorWebAssemblyStyleContributor)); });

            options
                .ScriptBundles
                .Add(BlazorWebAssemblyBundles.Scripts.Global,
                    bundle => { bundle.AddContributors(typeof(BlazorWebAssemblyScriptContributor)); });
        });
    }
}