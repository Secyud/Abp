using System.Security.Principal;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Security.Claims;

namespace Secyud.Abp.Features;

public class EditionFeaturesProvider(
    IFeaturesStore store,
    ICurrentPrincipalAccessor principalAccessor) : FeaturesProvider(store), ITransientDependency
{
    public override string Name => EditionFeatureValueProvider.ProviderName;

    protected ICurrentPrincipalAccessor PrincipalAccessor { get; } = principalAccessor;

    protected override Task<string?> NormalizeProviderKeyAsync(string? providerKey)
    {
        return Task.FromResult(providerKey ?? PrincipalAccessor.Principal.FindEditionId()?.ToString("N"));
    }
}