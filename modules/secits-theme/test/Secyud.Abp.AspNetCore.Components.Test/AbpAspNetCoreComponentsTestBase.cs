using Volo.Abp;
using Volo.Abp.Testing;

namespace Secyud.Abp.AspNetCore;

/* All test classes are derived from this class, directly or indirectly. */
public abstract class AbpAspNetCoreComponentsTestBase : AbpIntegratedTest<AbpAspNetCoreComponentsTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}