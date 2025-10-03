using JetBrains.Annotations;
using Volo.Abp.Features;

namespace Secyud.Abp.Features;

public static class EditionFeatureManagerExtensions
{
    public static Task<string?> GetOrNullForEditionAsync(this IFeatureManager featureManager, string name, Guid editionId, bool fallback = true)
    {
        return featureManager.GetOrNullAsync(name, EditionFeatureValueProvider.ProviderName, editionId.ToString(), fallback);
    }

    public static Task<List<FeatureNameValue>> GetAllForEditionAsync(this IFeatureManager featureManager, Guid editionId, bool fallback = true)
    {
        return featureManager.GetAllAsync(EditionFeatureValueProvider.ProviderName, editionId.ToString(), fallback);
    }

    public static Task<FeatureNameValueWithGrantedProvider> GetOrNullWithProviderForEditionAsync(this IFeatureManager featureManager, string name, Guid editionId, bool fallback = true)
    {
        return featureManager.GetOrNullWithProviderAsync(name, EditionFeatureValueProvider.ProviderName, editionId.ToString(), fallback);
    }

    public static Task<List<FeatureNameValueWithGrantedProvider>> GetAllWithProviderForEditionAsync(this IFeatureManager featureManager, Guid editionId, bool fallback = true)
    {
        return featureManager.GetAllWithProviderAsync(EditionFeatureValueProvider.ProviderName, editionId.ToString(), fallback);
    }

    public static Task SetForEditionAsync(this IFeatureManager featureManager, Guid editionId, string name, string? value, bool forceToSet = false)
    {
        return featureManager.SetAsync(name, value, EditionFeatureValueProvider.ProviderName, editionId.ToString(), forceToSet);
    }
}
