using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public interface IDynamicSettingDefinitionStoreInMemoryCache
{
    string? CacheStamp { get; set; }

    SemaphoreSlim SyncSemaphore { get; }

    DateTime? LastCheckTime { get; set; }

    Task FillAsync(List<SettingDefinitionRecord> settingRecords);

    SettingDefinition? GetSettingOrNull(string name);

    IReadOnlyList<SettingDefinition> GetSettings();
}
