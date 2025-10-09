using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Features;

public class NextTenantFeaturesProvider(
    IFeaturesStore store,
    ICurrentTenant currentTenant) : FeaturesProvider(store), ITransientDependency
{
    public static string ProviderName => "TENANT";

    public override string Name => ProviderName;

    protected ICurrentTenant CurrentTenant { get; } = currentTenant;

    protected override Task<string?> NormalizeProviderKeyAsync(string? providerKey)
    {
        return Task.FromResult(providerKey ?? CurrentTenant.Id?.ToString());
    }
}
