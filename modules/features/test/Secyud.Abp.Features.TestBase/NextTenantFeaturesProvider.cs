using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Features;

public class NextTenantFeaturesProvider : FeaturesProvider, ITransientDependency
{
    public static string ProviderName => "TENANT";

    public override string Name => ProviderName;

    protected ICurrentTenant CurrentTenant { get; }

    public NextTenantFeaturesProvider(
        IFeaturesStore store,
        ICurrentTenant currentTenant)
        : base(store)
    {
        CurrentTenant = currentTenant;
    }

    protected override Task<string?> NormalizeProviderKeyAsync(string? providerKey)
    {
        return Task.FromResult(providerKey ?? CurrentTenant.Id?.ToString());
    }
}
