using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Features;

[DependsOn(
    typeof(AbpFeaturesApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpFeaturesHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpFeaturesApplicationContractsModule).Assembly,
            FeaturesRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpFeaturesHttpApiClientModule>();
        });
    }
}
