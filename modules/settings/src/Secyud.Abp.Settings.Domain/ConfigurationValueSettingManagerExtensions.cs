using JetBrains.Annotations;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public static class ConfigurationValueSettingManagerExtensions
{
    public static Task<string?> GetOrNullConfigurationAsync(this ISettingManager settingManager, string name, bool fallback = true)
    {
        return settingManager.GetOrNullAsync(name, ConfigurationSettingValueProvider.ProviderName, null, fallback);
    }

    public static Task<List<SettingValue>> GetAllConfigurationAsync(this ISettingManager settingManager, bool fallback = true)
    {
        return settingManager.GetAllAsync(ConfigurationSettingValueProvider.ProviderName, null, fallback);
    }
}
