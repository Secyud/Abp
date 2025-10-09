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
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }
}