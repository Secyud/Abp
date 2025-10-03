using Secyud.Abp.Tenants.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Tenants;

[DependsOn(
    typeof(AbpTenantsEntityFrameworkCoreTestModule),
    typeof(AbpTenantsTestBaseModule))]
public class AbpSettingManagementDomainTestModule : AbpModule
{

}
