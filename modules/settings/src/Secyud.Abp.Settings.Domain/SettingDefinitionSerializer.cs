using System.Globalization;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public class SettingDefinitionSerializer(IGuidGenerator guidGenerator, ILocalizableStringSerializer localizableStringSerializer)
    : ISettingDefinitionSerializer, ITransientDependency
{
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; } = localizableStringSerializer;

    public virtual Task<SettingDefinitionRecord> SerializeAsync(SettingDefinition setting)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var displayName = LocalizableStringSerializer.Serialize(setting.DisplayName)!;
            var description = setting.Description is null ? null : LocalizableStringSerializer.Serialize(setting.Description);

            var record = new SettingDefinitionRecord(
                GuidGenerator.Create(),
                setting.Name,
                displayName,
                description,
                setting.DefaultValue,
                setting.IsVisibleToClients,
                SerializeProviders(setting.Providers),
                setting.IsInherited,
                setting.IsEncrypted);

            foreach (var property in setting.Properties)
            {
                record.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(record);
        }
    }

    public virtual async Task<List<SettingDefinitionRecord>> SerializeAsync(IEnumerable<SettingDefinition> settings)
    {
        var records = new List<SettingDefinitionRecord>();
        foreach (var setting in settings)
        {
            records.Add(await SerializeAsync(setting));
        }

        return records;
    }

    protected virtual string? SerializeProviders(ICollection<string> providers)
    {
        return providers.Count != 0
            ? providers.JoinAsString(",")
            : null;
    }
}