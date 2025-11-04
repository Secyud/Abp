using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Identities;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpIdentitiesInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIdentitiesInstallerModule>();
        });
    }
}
