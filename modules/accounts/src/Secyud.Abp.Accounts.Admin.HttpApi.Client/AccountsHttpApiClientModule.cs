using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Account;
using Secyud.Abp.Accounts;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

[DependsOn(
    typeof(AbpAccountsAdminApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpAccountsAdminHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(AbpAccountsAdminApplicationContractsModule).Assembly,
            AccountsAdminRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountsAdminHttpApiClientModule>();
        });

    }
}