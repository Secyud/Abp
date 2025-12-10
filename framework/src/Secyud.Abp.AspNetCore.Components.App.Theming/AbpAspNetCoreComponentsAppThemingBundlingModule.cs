using Secyud.Abp.AspNetCore.Components.Bundling;
using Secyud.Abp.Secits.Blazor;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Secyud.Abp.AspNetCore.Components;

[DependsOn(
    typeof(AbpSecitsBlazorModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule)
)]
public class AbpAspNetCoreComponentsAppThemingBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.GlobalAssets.Enabled = true;
            options.GlobalAssets.GlobalStyleBundleName = BlazorAppStandardBundles.Styles.Global;
            options.GlobalAssets.GlobalScriptBundleName = BlazorAppStandardBundles.Scripts.Global;

            options
                .StyleBundles
                .Add(BlazorAppStandardBundles.Styles.Global,
                    bundle => { bundle.AddContributors(typeof(BlazorAppStyleContributor)); });

            options
                .ScriptBundles
                .Add(BlazorAppStandardBundles.Scripts.Global,
                    bundle => { bundle.AddContributors(typeof(BlazorAppScriptContributor)); });

            options.MinificationIgnoredFiles.Add(
                "_content/Microsoft.AspNetCore.Components.App.Authentication/AuthenticationService.js");
        });
    }
}