using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace Secyud.Abp.Features;

public class FeatureStore(IFeaturesStore featuresStore) : IFeatureStore, ITransientDependency
{
    protected IFeaturesStore FeaturesStore { get; } = featuresStore;

    public virtual Task<string?> GetOrNullAsync(
        string name,
        string? providerName,
        string? providerKey)
    {
        return FeaturesStore.GetOrNullAsync(name, providerName, providerKey);
    }
}
