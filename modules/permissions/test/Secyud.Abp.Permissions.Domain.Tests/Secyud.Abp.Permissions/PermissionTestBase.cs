using Secyud.Abp.Permissions.EntityFrameworkCore;

namespace Secyud.Abp.Permissions;

public abstract class PermissionTestBase : PermissionsTestBase<AbpPermissionsTestModule>
{
    protected virtual void UsingDbContext(Action<IPermissionsDbContext> action)
    {
        using (var dbContext = GetRequiredService<IPermissionsDbContext>())
        {
            action.Invoke(dbContext);
        }
    }

    protected virtual T UsingDbContext<T>(Func<IPermissionsDbContext, T> action)
    {
        using (var dbContext = GetRequiredService<IPermissionsDbContext>())
        {
            return action.Invoke(dbContext);
        }
    }
}
