using JetBrains.Annotations;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public static class DefaultValueSettingManagerExtensions
{
    public static Task<string?> GetOrNullDefaultAsync(this ISettingManager settingManager, string name, bool fallback = true)
    {
        return settingManager.GetOrNullAsync(name, DefaultValueSettingValueProvider.ProviderName, null, fallback);
    }

    public static Task<List<SettingValue>> GetAllDefaultAsync(this ISettingManager settingManager, bool fallback = true)
    {
        return settingManager.GetAllAsync(DefaultValueSettingValueProvider.ProviderName, null, fallback);
    }
}
