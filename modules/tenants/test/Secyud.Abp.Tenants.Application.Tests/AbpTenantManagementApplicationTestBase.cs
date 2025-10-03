using Secyud.Abp.Tenants.EntityFrameworkCore;

namespace Secyud.Abp.Tenants;

public abstract class AbpTenantsApplicationTestBase : TenantsTestBase<AbpTenantsApplicationTestModule>
{
    protected virtual void UsingDbContext(Action<ITenantsDbContext> action)
    {
        using (var dbContext = GetRequiredService<ITenantsDbContext>())
        {
            action.Invoke(dbContext);
        }
    }

    protected virtual T UsingDbContext<T>(Func<ITenantsDbContext, T> action)
    {
        using (var dbContext = GetRequiredService<ITenantsDbContext>())
        {
            return action.Invoke(dbContext);
        }
    }
}
