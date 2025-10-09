using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public static class UserSettingManagerExtensions
{
    public static Task<string?> GetOrNullForUserAsync(this ISettingManager settingManager, string name, Guid userId, bool fallback = true)
    {
        return settingManager.GetOrNullAsync(name, UserSettingValueProvider.ProviderName, userId.ToString(), fallback);
    }

    public static Task<string?> GetOrNullForCurrentUserAsync(this ISettingManager settingManager, string name, bool fallback = true)
    {
        return settingManager.GetOrNullAsync(name, UserSettingValueProvider.ProviderName, null, fallback);
    }

    public static Task<List<SettingValue>> GetAllForUserAsync(this ISettingManager settingManager, Guid userId, bool fallback = true)
    {
        return settingManager.GetAllAsync(UserSettingValueProvider.ProviderName, userId.ToString(), fallback);
    }

    public static Task<List<SettingValue>> GetAllForCurrentUserAsync(this ISettingManager settingManager, bool fallback = true)
    {
        return settingManager.GetAllAsync(UserSettingValueProvider.ProviderName, null, fallback);
    }

    public static Task SetForUserAsync(this ISettingManager settingManager, Guid userId, string name, string? value, bool forceToSet = false)
    {
        return settingManager.SetAsync(name, value, UserSettingValueProvider.ProviderName, userId.ToString(), forceToSet);
    }

    public static Task SetForCurrentUserAsync(this ISettingManager settingManager, string name, string? value, bool forceToSet = false)
    {
        return settingManager.SetAsync(name, value, UserSettingValueProvider.ProviderName, null, forceToSet);
    }
}