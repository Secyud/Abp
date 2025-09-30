using JetBrains.Annotations;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public interface ISettingManager
{
    Task<string?> GetOrNullAsync(string name, string? providerName, string? providerKey, bool fallback = true);

    Task<List<SettingValue>> GetAllAsync(string? providerName, string? providerKey, bool fallback = true);

    Task SetAsync(string name, string? value, string? providerName, string? providerKey, bool forceToSet = false);

    Task DeleteAsync(string? providerName, string? providerKey);
}