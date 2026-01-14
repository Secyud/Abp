using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Secyud.Abp.Permissions.EntityFrameworkCore;

public class EfCorePermissionGroupDefinitionRecordRepository(IDbContextProvider<IPermissionsDbContext> dbContextProvider) :
    EfCoreRepository<IPermissionsDbContext, PermissionGroupDefinitionRecord, Guid>(dbContextProvider),
    IPermissionGroupDefinitionRecordRepository
{
}