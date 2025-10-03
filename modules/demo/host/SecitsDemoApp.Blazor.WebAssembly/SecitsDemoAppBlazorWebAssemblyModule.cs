using Secyud.Abp.AspNetCore;
using Volo.Abp.Modularity;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblySecitsThemeModule)
    )]
public class SecitsDemoAppBlazorWebAssemblyModule : AbpModule
{

}
