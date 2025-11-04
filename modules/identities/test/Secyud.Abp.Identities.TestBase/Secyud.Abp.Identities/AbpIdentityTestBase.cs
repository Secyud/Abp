using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Secyud.Abp.Identities;

public abstract class AbpIdentityTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
