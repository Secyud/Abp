using System.Globalization;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Secyud.Abp.Features;

public class FeatureDefinitionSerializer(
    IGuidGenerator guidGenerator,
    ILocalizableStringSerializer localizableStringSerializer,
    StringValueTypeSerializer stringValueTypeSerializer)
    : IFeatureDefinitionSerializer, ITransientDependency
{
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; } = localizableStringSerializer;
    protected StringValueTypeSerializer StringValueTypeSerializer { get; } = stringValueTypeSerializer;

    public async Task<(FeatureGroupDefinitionRecord[], FeatureDefinitionRecord[])> SerializeAsync(IEnumerable<FeatureGroupDefinition> featureGroups)
    {
        var featureGroupRecords = new List<FeatureGroupDefinitionRecord>();
        var featureRecords = new List<FeatureDefinitionRecord>();

        foreach (var featureGroup in featureGroups)
        {
            featureGroupRecords.Add(await SerializeAsync(featureGroup));

            foreach (var feature in featureGroup.GetFeaturesWithChildren())
            {
                featureRecords.Add(await SerializeAsync(feature, featureGroup));
            }
        }

        return (featureGroupRecords.ToArray(), featureRecords.ToArray());
    }

    public Task<FeatureGroupDefinitionRecord> SerializeAsync(FeatureGroupDefinition featureGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var featureGroupRecord = new FeatureGroupDefinitionRecord(
                GuidGenerator.Create(),
                featureGroup.Name,
                LocalizableStringSerializer.Serialize(featureGroup.DisplayName)
            );

            foreach (var property in featureGroup.Properties)
            {
                featureGroupRecord.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(featureGroupRecord);
        }
    }

    public Task<FeatureDefinitionRecord> SerializeAsync(FeatureDefinition feature, FeatureGroupDefinition? featureGroup)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var featureRecord = new FeatureDefinitionRecord(
                GuidGenerator.Create(),
                featureGroup?.Name,
                feature.Name,
                feature.Parent?.Name,
                LocalizableStringSerializer.Serialize(feature.DisplayName),
                feature.Description is null ? null : LocalizableStringSerializer.Serialize(feature.Description),
                feature.DefaultValue,
                feature.IsVisibleToClients,
                feature.IsAvailableToHost,
                SerializeProviders(feature.AllowedProviders),
                SerializeStringValueType(feature.ValueType)
            );

            foreach (var property in feature.Properties)
            {
                featureRecord.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(featureRecord);
        }
    }

    protected virtual string? SerializeProviders(ICollection<string> providers)
    {
        return providers.Any()
            ? providers.JoinAsString(",")
            : null;
    }

    protected virtual string? SerializeStringValueType(IStringValueType? stringValueType)
    {
        return stringValueType is null ? null : StringValueTypeSerializer.Serialize(stringValueType);
    }
}