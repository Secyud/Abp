using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Secyud.Abp.Permissions;

public abstract class PermissionsTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
