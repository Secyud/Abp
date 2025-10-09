using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Tenants;

[DependsOn(
    typeof(AbpTenantsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpTenantsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpTenantsApplicationContractsModule).Assembly,
            TenantsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTenantsHttpApiClientModule>();
        });
    }
}
