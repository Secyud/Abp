using Volo.Abp.Features;

namespace Secyud.Abp.Features;

public interface IFeatureDefinitionSerializer
{
    Task<(FeatureGroupDefinitionRecord[], FeatureDefinitionRecord[])> SerializeAsync(IEnumerable<FeatureGroupDefinition> featureGroups);

    Task<FeatureGroupDefinitionRecord> SerializeAsync(FeatureGroupDefinition featureGroup);

    Task<FeatureDefinitionRecord> SerializeAsync(FeatureDefinition feature, FeatureGroupDefinition? featureGroup);
}
