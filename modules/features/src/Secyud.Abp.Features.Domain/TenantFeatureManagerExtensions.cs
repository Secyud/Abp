using JetBrains.Annotations;
using Volo.Abp.Features;

namespace Secyud.Abp.Features;

public static class TenantFeatureManagerExtensions
{
    public static Task<string?> GetOrNullForTenantAsync(this IFeatureManager featureManager, string name, Guid tenantId, bool fallback = true)
    {
        return featureManager.GetOrNullAsync(name, TenantFeatureValueProvider.ProviderName, tenantId.ToString(), fallback);
    }

    public static Task<List<FeatureNameValue>> GetAllForTenantAsync(this IFeatureManager featureManager, Guid tenantId, bool fallback = true)
    {
        return featureManager.GetAllAsync(TenantFeatureValueProvider.ProviderName, tenantId.ToString(), fallback);
    }

    public static Task<FeatureNameValueWithGrantedProvider> GetOrNullWithProviderForTenantAsync(this IFeatureManager featureManager, string name, Guid tenantId, bool fallback = true)
    {
        return featureManager.GetOrNullWithProviderAsync(name, TenantFeatureValueProvider.ProviderName, tenantId.ToString(), fallback);
    }

    public static Task<List<FeatureNameValueWithGrantedProvider>> GetAllWithProviderForTenantAsync(this IFeatureManager featureManager, Guid tenantId, bool fallback = true)
    {
        return featureManager.GetAllWithProviderAsync(TenantFeatureValueProvider.ProviderName, tenantId.ToString(), fallback);
    }

    public static Task SetForTenantAsync(this IFeatureManager featureManager, Guid tenantId, string name, string? value, bool forceToSet = false)
    {
        return featureManager.SetAsync(name, value, TenantFeatureValueProvider.ProviderName, tenantId.ToString(), forceToSet);
    }
}
