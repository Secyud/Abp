using JetBrains.Annotations;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public static class GlobalSettingManagerExtensions
{
    public static Task<string?> GetOrNullGlobalAsync(this ISettingManager settingManager, string name, bool fallback = true)
    {
        return settingManager.GetOrNullAsync(name, GlobalSettingValueProvider.ProviderName, null, fallback);
    }

    public static Task<List<SettingValue>> GetAllGlobalAsync(this ISettingManager settingManager, bool fallback = true)
    {
        return settingManager.GetAllAsync(GlobalSettingValueProvider.ProviderName, null, fallback);
    }

    public static Task SetGlobalAsync(this ISettingManager settingManager, string name, string? value)
    {
        return settingManager.SetAsync(name, value, GlobalSettingValueProvider.ProviderName, null);
    }
}
