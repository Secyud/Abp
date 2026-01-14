namespace Secyud.Abp.Features;

public interface IFeatureDefinitionManager
{
    Task<FeatureDefinition> GetAsync(string name);

    Task<IReadOnlyList<FeatureDefinition>> GetAllAsync();

    Task<FeatureDefinition?> GetOrNullAsync(string name);

    Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync();
}
