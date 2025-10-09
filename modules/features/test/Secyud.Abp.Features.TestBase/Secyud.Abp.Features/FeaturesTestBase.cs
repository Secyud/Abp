using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Secyud.Abp.Features;

public abstract class FeaturesTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
