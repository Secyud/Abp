using Secyud.Abp.Account;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Accounts;

[DependsOn(
    typeof(AbpAccountsSharedApplicationModule),
    typeof(AbpAccountsAdminApplicationContractsModule)
)]
public class AbpAccountsAdminApplicationModule : AbpModule
{
}
