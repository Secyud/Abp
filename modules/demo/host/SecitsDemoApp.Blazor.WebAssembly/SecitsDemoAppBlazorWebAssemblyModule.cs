using SecitsDemoApp;
using Volo.Abp.Modularity;

namespace Secyud.Abp.AspNetCore;

[DependsOn(
    typeof(SecitsDemoAppHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblySecitsThemeModule)
    )]
public class SecitsDemoAppBlazorWebAssemblyModule : AbpModule
{

}
