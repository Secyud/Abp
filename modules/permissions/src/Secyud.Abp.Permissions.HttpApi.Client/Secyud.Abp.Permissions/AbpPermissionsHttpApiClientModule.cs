using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Permissions;

[DependsOn(
    typeof(AbpPermissionsApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpPermissionsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpPermissionsApplicationContractsModule).Assembly,
            PermissionsRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpPermissionsHttpApiClientModule>();
        });
    }
}
