using Secyud.Abp.AspNetCore.Theming;
using Volo.Abp.Modularity;

namespace Secyud.Abp.AspNetCore;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule)
)]
public class AbpAspNetCoreComponentsWebBasicThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<SecitsDefaultTheme>();
            options.DefaultThemeName ??= SecitsDefaultTheme.Name;
        });
    }
}