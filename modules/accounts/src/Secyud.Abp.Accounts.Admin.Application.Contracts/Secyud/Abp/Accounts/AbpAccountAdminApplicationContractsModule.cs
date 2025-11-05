using Secyud.Abp.Account;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Accounts;

[DependsOn(
    typeof(AbpAccountsSharedApplicationContractsModule)
    )]
public class AbpAccountsAdminApplicationContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
