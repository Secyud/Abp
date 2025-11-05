using Secyud.Abp.Identities;
using Secyud.Abp.Settings;
using Volo.Abp.Application;
using Volo.Abp.Emailing;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.Account;

[DependsOn(
    typeof(AbpAccountsSharedApplicationContractsModule),
    typeof(AbpEmailingModule),
    typeof(AbpIdentitiesDomainModule),
    typeof(AbpSettingsDomainModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpUiNavigationModule),
    typeof(AbpJsonModule)
    )]
public class AbpAccountsSharedApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }
}
