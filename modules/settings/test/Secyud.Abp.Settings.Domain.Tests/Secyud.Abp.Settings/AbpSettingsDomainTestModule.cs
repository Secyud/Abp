using Secyud.Abp.Settings.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace Secyud.Abp.Settings;

[DependsOn(
    typeof(AbpSettingsEntityFrameworkCoreTestModule),
    typeof(AbpUsersAbstractionModule))]
public class AbpSettingsDomainTestModule : AbpModule
{
}