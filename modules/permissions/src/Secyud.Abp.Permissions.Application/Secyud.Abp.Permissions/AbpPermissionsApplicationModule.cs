using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpPermissionsDomainModule),
    typeof(AbpPermissionsApplicationContractsModule),
    typeof(AbpDddApplicationModule)
)]
public class AbpPermissionsApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}