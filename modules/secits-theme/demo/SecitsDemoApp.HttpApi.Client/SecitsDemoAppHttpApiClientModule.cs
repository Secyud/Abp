using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace SecitsDemoApp;

[DependsOn(
    typeof(SecitsDemoAppApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class SecitsDemoAppHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(SecitsDemoAppApplicationContractsModule).Assembly,
            SecitsDemoAppRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<SecitsDemoAppHttpApiClientModule>();
        });

    }
}
