using Microsoft.Extensions.Options;

namespace Secyud.Abp.Features;

public class FeatureChecker(
    IOptions<AbpFeatureOptions> options,
    IServiceProvider serviceProvider,
    IFeatureDefinitionManager featureDefinitionManager,
    IFeatureValueProviderManager featureValueProviderManager)
    : FeatureCheckerBase
{
    protected AbpFeatureOptions Options { get; } = options.Value;
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;
    protected IFeatureDefinitionManager FeatureDefinitionManager { get; } = featureDefinitionManager;
    protected IFeatureValueProviderManager FeatureValueProviderManager { get; } = featureValueProviderManager;

    public override async Task<string?> GetOrNullAsync(string name)
    {
        var featureDefinition = await FeatureDefinitionManager.GetAsync(name);
        var providers = FeatureValueProviderManager.ValueProviders
            .Reverse();

        if (featureDefinition.AllowedProviders.Count != 0)
        {
            providers = providers.Where(p => featureDefinition.AllowedProviders.Contains(p.Name));
        }

        return await GetOrNullValueFromProvidersAsync(providers, featureDefinition);
    }

    protected virtual async Task<string?> GetOrNullValueFromProvidersAsync(
        IEnumerable<IFeatureValueProvider> providers,
        FeatureDefinition feature)
    {
        foreach (var provider in providers)
        {
            var value = await provider.GetOrNullAsync(feature);
            if (value != null)
            {
                return value;
            }
        }

        return null;
    }
}
