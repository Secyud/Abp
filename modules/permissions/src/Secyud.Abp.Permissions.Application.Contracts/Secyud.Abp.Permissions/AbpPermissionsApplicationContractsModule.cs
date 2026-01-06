using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpPermissionsDomainSharedModule)
    )]
public class AbpPermissionsApplicationContractsModule : AbpModule
{

}
