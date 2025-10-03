using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Tenants;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class TenantsInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<TenantsInstallerModule>();
        });
    }
}
