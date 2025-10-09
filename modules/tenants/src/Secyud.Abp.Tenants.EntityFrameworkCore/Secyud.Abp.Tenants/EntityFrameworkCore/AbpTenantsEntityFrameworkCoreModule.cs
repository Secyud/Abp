using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Tenants.EntityFrameworkCore;

[DependsOn(typeof(AbpTenantsDomainModule))]
[DependsOn(typeof(AbpEntityFrameworkCoreModule))]
public class AbpTenantsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TenantsDbContext>(options =>
        {
            options.AddDefaultRepositories<ITenantsDbContext>();
        });
    }
}
