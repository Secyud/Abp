using Secyud.Abp.Secits.Blazor;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.AspNetCore.Components;

[DependsOn(
    typeof(AbpSecitsBlazorModule),
    typeof(AbpUiNavigationModule)
)]
public class AbpAspNetCoreComponentsWebThemingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDynamicLayoutComponentOptions>(options =>
        {
            options.Components.Add(typeof(AbpAuthenticationState), null);
        });
    }
}