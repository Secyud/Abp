using JetBrains.Annotations;

namespace Secyud.Abp.Features;

public interface IFeatureManager
{
    Task<string?> GetOrNullAsync(string name, string? providerName, string? providerKey, bool fallback = true);

    Task<List<FeatureNameValue>> GetAllAsync(string? providerName, string? providerKey, bool fallback = true);

    Task<FeatureNameValueWithGrantedProvider> GetOrNullWithProviderAsync(string name, string? providerName, string? providerKey, bool fallback = true);

    Task<List<FeatureNameValueWithGrantedProvider>> GetAllWithProviderAsync(string? providerName, string? providerKey, bool fallback = true);

    Task SetAsync(string name, string? value, string? providerName, string? providerKey, bool forceToSet = false);

    Task DeleteAsync(string? providerName, string? providerKey);
}
