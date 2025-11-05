using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Identities;
using Volo.Abp;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.StaticProxying;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Accounts;

[DependsOn(
    typeof(AbpAccountsPublicApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpAccountPublicHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(typeof(AbpAccountsPublicApplicationContractsModule).Assembly,
            AccountProPublicRemoteServiceConsts.RemoteServiceName);

        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<AbpAccountPublicHttpApiClientModule>(); });

        Configure<AbpHttpClientStaticProxyingOptions>(options => { options.BindingFromQueryTypes.Add(typeof(GetIdentitySecurityLogListInput)); });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}