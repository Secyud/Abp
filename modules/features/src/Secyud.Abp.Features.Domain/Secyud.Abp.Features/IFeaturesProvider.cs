using Volo.Abp.Features;

namespace Secyud.Abp.Features;

public interface IFeaturesProvider
{
    string Name { get; }

    //TODO: Other better method name.
    bool Compatible(string? providerName);

    //TODO: Other better method name.
    Task<IAsyncDisposable> HandleContextAsync(string? providerName, string? providerKey);

    Task<string?> GetOrNullAsync(FeatureDefinition feature, string? providerKey);

    Task SetAsync(FeatureDefinition feature, string? value, string? providerKey);

    Task ClearAsync(FeatureDefinition feature, string? providerKey);
}
