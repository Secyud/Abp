using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Tenants.EntityFrameworkCore;

public class LazyLoadTests : LazyLoadTests<AbpTenantsEntityFrameworkCoreTestModule>
{
    protected override void BeforeAddApplication(IServiceCollection services)
    {
        services.Configure<AbpDbContextOptions>(options =>
        {
            options.PreConfigure<TenantsDbContext>(context =>
            {
                context.DbContextOptions.UseLazyLoadingProxies();
            });
        });
    }
}
