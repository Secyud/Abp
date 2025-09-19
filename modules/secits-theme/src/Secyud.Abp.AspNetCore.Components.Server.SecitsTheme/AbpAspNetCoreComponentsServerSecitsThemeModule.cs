using Secyud.Abp.AspNetCore.Bundling;
using Secyud.Abp.AspNetCore.Components;
using Secyud.Abp.AspNetCore.Components.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Secyud.Abp.AspNetCore;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebSecitsThemeModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule)
)]
public class AbpAspNetCoreComponentsServerSecitsThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BlazorSecitsThemeBundles.Styles.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(BlazorStandardBundles.Styles.Global)
                        .AddContributors(typeof(BlazorSecitsThemeStyleContributor));
                });

            options
                .ScriptBundles
                .Add(BlazorSecitsThemeBundles.Scripts.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(BlazorStandardBundles.Scripts.Global)
                        .AddContributors(typeof(BlazorSecitsThemeScriptContributor));
                });
        });
    }
}