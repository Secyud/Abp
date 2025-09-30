using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Settings;

[DependsOn(
    typeof(AbpSettingsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule)
)]
public class AbpSettingsApplicationContractsModule : AbpModule
{
}


