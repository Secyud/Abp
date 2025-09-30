using Volo.Abp.Modularity;

namespace Secyud.Abp.Settings;

[DependsOn(
    typeof(AbpSettingsApplicationModule),
    typeof(AbpSettingsDomainTestModule)
)]
public class AbpSettingsApplicationTestModule : AbpModule
{
}