using Volo.Abp.Domain.Repositories;

namespace Secyud.Abp.Permissions;

public interface IPermissionGroupDefinitionRecordRepository : IRepository<PermissionGroupDefinitionRecord, Guid>
{
}