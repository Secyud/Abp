using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Identities.EntityFrameworkCore;

public class LazyLoadingTests : LazyLoadingTests<AbpIdentitiesEntityFrameworkCoreTestModule>
{
    protected override void BeforeAddApplication(IServiceCollection services)
    {
        services.Configure<AbpDbContextOptions>(options =>
        {
            options.PreConfigure<IdentitiesDbContext>(context =>
            {
                context.DbContextOptions.UseLazyLoadingProxies();
            });

            options.PreConfigure<IdentitiesDbContext>(context =>
            {
                context.DbContextOptions.UseLazyLoadingProxies();
            });
        });
    }
}
