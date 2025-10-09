using Volo.Abp.Application;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace Secyud.Abp.Settings;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpSettingsDomainModule),
    typeof(AbpSettingsApplicationContractsModule),
    typeof(AbpEmailingModule),
    typeof(AbpTimingModule),
    typeof(AbpUsersAbstractionModule)
)]
public class AbpSettingsApplicationModule : AbpModule
{
}
