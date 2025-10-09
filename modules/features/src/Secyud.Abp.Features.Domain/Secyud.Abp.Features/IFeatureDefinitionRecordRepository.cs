using Volo.Abp.Domain.Repositories;

namespace Secyud.Abp.Features;

public interface IFeatureDefinitionRecordRepository : IBasicRepository<FeatureDefinitionRecord, Guid>
{
    Task<FeatureDefinitionRecord?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);
}
