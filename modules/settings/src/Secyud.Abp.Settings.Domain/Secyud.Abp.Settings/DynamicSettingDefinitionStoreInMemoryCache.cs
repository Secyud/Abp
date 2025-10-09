using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public class DynamicSettingDefinitionStoreInMemoryCache(ILocalizableStringSerializer localizableStringSerializer)
    : IDynamicSettingDefinitionStoreInMemoryCache, ISingletonDependency
{
    public string? CacheStamp { get; set; }

    protected IDictionary<string, SettingDefinition> SettingDefinitions { get; } = new Dictionary<string, SettingDefinition>();
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; } = localizableStringSerializer;

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    public DateTime? LastCheckTime { get; set; }

    public Task FillAsync(List<SettingDefinitionRecord> settingRecords)
    {
        SettingDefinitions.Clear();

        foreach (var record in settingRecords)
        {
            var settingDefinition = new SettingDefinition(
                record.Name,
                record.DefaultValue,
                LocalizableStringSerializer.Deserialize(record.DisplayName),
                record.Description != null ? LocalizableStringSerializer.Deserialize(record.Description) : null,
                record.IsVisibleToClients,
                record.IsInherited,
                record.IsEncrypted);

            if (!record.Providers.IsNullOrWhiteSpace())
            {
                settingDefinition.Providers.AddRange(record.Providers.Split(','));
            }

            foreach (var (key, value) in record.ExtraProperties)
            {
                if (value is null) continue;
                settingDefinition.WithProperty(key, value);
            }

            SettingDefinitions[record.Name] = settingDefinition;
        }

        return Task.CompletedTask;
    }

    public SettingDefinition? GetSettingOrNull(string name)
    {
        return SettingDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<SettingDefinition> GetSettings()
    {
        return SettingDefinitions.Values.ToList();
    }
}