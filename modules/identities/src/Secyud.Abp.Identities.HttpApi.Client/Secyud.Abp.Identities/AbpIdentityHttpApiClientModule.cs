using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.StaticProxying;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Identities;

[DependsOn(
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpIdentityHttpApiClientModule : AbpModule
{
    public static readonly string Development = nameof(Development);

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(typeof(AbpIdentityApplicationContractsModule).Assembly,
            IdentitiesRemoteServiceConsts.RemoteServiceName);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIdentityHttpApiClientModule>();
        });

        Configure<AbpHttpClientStaticProxyingOptions>(options =>
        {
            options.BindingFromQueryTypes.Add(typeof(GetIdentitySecurityLogListInput));
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
