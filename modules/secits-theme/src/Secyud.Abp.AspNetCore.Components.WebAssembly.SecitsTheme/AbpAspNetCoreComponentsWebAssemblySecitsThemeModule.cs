using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Http.Client.IdentityModel.WebAssembly;
using Volo.Abp.Modularity;

namespace Secyud.Abp.AspNetCore;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(AbpAspNetCoreComponentsWebSecitsThemeModule),
    typeof(AbpHttpClientIdentityModelWebAssemblyModule)
)]
public class AbpAspNetCoreComponentsWebAssemblySecitsThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}