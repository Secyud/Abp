using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Permissions.EntityFrameworkCore;

public class EfCorePermissionDefinitionRecordRepository(IDbContextProvider<IPermissionsDbContext> dbContextProvider) :
    EfCoreRepository<IPermissionsDbContext, PermissionDefinitionRecord, Guid>(dbContextProvider),
    IPermissionDefinitionRecordRepository
{
    public virtual async Task<PermissionDefinitionRecord?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).FirstOrDefaultAsync(
            r => r.Name == name, cancellationToken);
    }

    public async Task<List<PermissionDefinitionRecord>> GetListAsync(string prefix,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(u => u.Name.StartsWith(prefix))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}