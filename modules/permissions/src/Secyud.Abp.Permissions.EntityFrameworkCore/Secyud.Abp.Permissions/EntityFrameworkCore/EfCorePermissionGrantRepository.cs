using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Permissions.EntityFrameworkCore;

public class EfCorePermissionGrantRepository(IDbContextProvider<IPermissionsDbContext> dbContextProvider) :
    EfCoreRepository<IPermissionsDbContext, PermissionGrant, Guid>(dbContextProvider),
    IPermissionGrantRepository
{
    public virtual async Task<PermissionGrant?> FindAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .FirstOrDefaultAsync(s =>
                s.Name == name &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey,
                GetCancellationToken(cancellationToken)
            );
    }

    public virtual async Task<List<PermissionGrant>> GetListAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<PermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                ((IEnumerable<string>)names).Contains(s.Name) &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }
}
