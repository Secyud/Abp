using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Secyud.Abp.Tenants;

public abstract class TenantsTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
