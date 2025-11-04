using Secyud.Abp.Identities.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;

namespace Secyud.Abp.Identities;

public abstract class AbpIdentityExtendedTestBase<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected virtual IdentityUser GetUser(string userName)
    {
        var user = UsingDbContext(context => context.Users.FirstOrDefault(u => u.UserName == userName));
        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        return user;
    }

    protected virtual IdentityUser? FindUser(string userName)
    {
        return UsingDbContext(context => context.Users.FirstOrDefault(u => u.UserName == userName));
    }

    protected virtual void UsingDbContext(Action<IIdentitiesDbContext> action)
    {
        using var dbContext = GetRequiredService<IIdentitiesDbContext>();
        action.Invoke(dbContext);
    }

    protected virtual T UsingDbContext<T>(Func<IIdentitiesDbContext, T> action)
    {
        using var dbContext = GetRequiredService<IIdentitiesDbContext>();
        return action.Invoke(dbContext);
    }
}
