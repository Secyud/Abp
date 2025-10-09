using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpPermissionsApplicationModule),
    typeof(AbpPermissionsTestModule)
)]
public class AbpPermissionsApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysAllowAuthorization();
    }
}
