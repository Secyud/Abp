using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Users;

public abstract class EfCoreUserRepositoryBase<TDbContext, TUser>(IDbContextProvider<TDbContext> dbContextProvider)
    : EfCoreRepository<TDbContext, TUser, Guid>(dbContextProvider), IUserRepository<TUser>
    where TDbContext : IEfCoreDbContext
    where TUser : class, IUser
{
    public async Task<TUser?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).OrderBy(x => x.Id).FirstOrDefaultAsync(u => u.UserName == userName, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<TUser>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(u => ids.Contains(u.Id))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<List<TUser>> SearchAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.UserName!.Contains(filter!) ||
                    u.Email!.Contains(filter!) ||
                    u.Name!.Contains(filter!) ||
                    u.Surname!.Contains(filter!)
            )
            .OrderBy(sorting.IsNullOrEmpty() ? nameof(IUser.UserName) : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<long> GetCountAsync(
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                u =>
                    u.UserName!.Contains(filter!) ||
                    u.Email!.Contains(filter!) ||
                    u.Name!.Contains(filter!) ||
                    u.Surname!.Contains(filter!)
            )
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }
}