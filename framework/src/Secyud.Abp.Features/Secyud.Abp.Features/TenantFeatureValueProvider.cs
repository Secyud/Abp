using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Features;

public class TenantFeatureValueProvider(IFeatureStore featureStore, ICurrentTenant currentTenant)
    : FeatureValueProviderBase(featureStore)
{
    public const string ProviderName = "T";

    public override string Name => ProviderName;

    protected ICurrentTenant CurrentTenant { get; } = currentTenant;

    public override async Task<string?> GetOrNullAsync(FeatureDefinition feature)
    {
        return await FeatureStore.GetOrNullAsync(feature.Name, Name, CurrentTenant.Id?.ToString());
    }
}
