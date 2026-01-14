using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Features;

public abstract class FeatureValueProviderBase(IFeatureStore featureStore) : IFeatureValueProvider, ITransientDependency
{
    public abstract string Name { get; }

    protected IFeatureStore FeatureStore { get; } = featureStore;

    public abstract Task<string?> GetOrNullAsync(FeatureDefinition feature);
}