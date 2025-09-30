using Volo.Abp.Modularity;

namespace Secyud.Abp.Settings;

[DependsOn(
    typeof(AbpSettingsDomainModule),
    typeof(AbpSettingsTestBaseModule)
)]
public class AbpSettingsDomainTestModule : AbpModule
{

}
