using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Identities.EntityFrameworkCore;

public class EfCoreIdentityClaimTypeRepository(IDbContextProvider<IIdentitiesDbContext> dbContextProvider)
    : EfCoreRepository<IIdentitiesDbContext, IdentityClaimType, Guid>(dbContextProvider), IIdentityClaimTypeRepository
{
    public virtual async Task<bool> AnyAsync(
        string name,
        Guid? ignoredId = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(ignoredId != null, ct => ct.Id != ignoredId)
            .CountAsync(ct => ct.Name == name, GetCancellationToken(cancellationToken)) > 0;
    }

    public virtual async Task<List<IdentityClaimType>> GetListAsync(
        string? sorting,
        int maxResultCount,
        int skipCount,
        string? filter,
        CancellationToken cancellationToken = default)
    {
        var identityClaimTypes = await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), u => u.Name.Contains(filter!))
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(IdentityClaimType.CreationTime) + " desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));

        return identityClaimTypes;
    }

    public virtual async Task<long> GetCountAsync(
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), u => u.Name.Contains(filter!))
            .LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<IdentityClaimType>> GetListByNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => names.Contains(x.Name))
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}