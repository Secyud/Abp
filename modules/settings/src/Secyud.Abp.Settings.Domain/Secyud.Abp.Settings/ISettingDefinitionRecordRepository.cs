using Volo.Abp.Domain.Repositories;

namespace Secyud.Abp.Settings;

public interface ISettingDefinitionRecordRepository : IBasicRepository<SettingDefinitionRecord, Guid>
{
    Task<SettingDefinitionRecord?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}
