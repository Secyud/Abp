using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Settings;

[DependsOn(
    typeof(AbpSettingsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpSettingsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpSettingsApplicationContractsModule).Assembly,
            SettingsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpSettingsHttpApiClientModule>();
        });
    }
}
