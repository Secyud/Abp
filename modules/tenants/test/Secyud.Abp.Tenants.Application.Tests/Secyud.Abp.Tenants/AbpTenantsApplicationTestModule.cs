using Microsoft.Extensions.DependencyInjection;
using Secyud.Abp.Tenants.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Tenants;

[DependsOn(
    typeof(AbpTenantsApplicationModule),
    typeof(AbpTenantsEntityFrameworkCoreTestModule))]
public class AbpTenantsApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysAllowAuthorization();
    }
}
