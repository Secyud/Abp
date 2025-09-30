using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public interface ISettingsProvider
{
    string Name { get; }

    Task<string?> GetOrNullAsync(SettingDefinition setting, string? providerKey);

    Task SetAsync(SettingDefinition setting, string value, string? providerKey);

    Task ClearAsync(SettingDefinition setting, string? providerKey);
}