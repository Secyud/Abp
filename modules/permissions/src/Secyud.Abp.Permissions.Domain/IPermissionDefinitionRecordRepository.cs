using Volo.Abp.Domain.Repositories;

namespace Secyud.Abp.Permissions;

public interface IPermissionDefinitionRecordRepository : IRepository<PermissionDefinitionRecord, Guid>
{
    Task<PermissionDefinitionRecord?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);
}