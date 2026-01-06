using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Authorization;

[DependsOn(
    typeof(AbpMultiTenancyAbstractionsModule)
)]
public class AbpAuthorizationAbstractionsModule : AbpModule
{

}