using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Secyud.Abp.Settings;

public class SettingsTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
