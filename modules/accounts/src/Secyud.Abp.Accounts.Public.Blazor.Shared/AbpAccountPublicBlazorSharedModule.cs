using Secyud.Abp.AspNetCore.Components;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Accounts;

[DependsOn(typeof(AbpAspNetCoreComponentsWebThemingModule))]
[DependsOn(typeof(AbpAccountsPublicApplicationContractsModule))]
public class AbpAccountsPublicBlazorSharedModule : AbpModule
{

}
