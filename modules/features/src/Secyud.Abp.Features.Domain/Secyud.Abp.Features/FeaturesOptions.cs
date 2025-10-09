using Volo.Abp.Collections;

namespace Secyud.Abp.Features;

public class FeaturesOptions
{
    public ITypeList<IFeaturesProvider> Providers { get; } = new TypeList<IFeaturesProvider>();

    public Dictionary<string, string> ProviderPolicies { get; } = new();

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveStaticFeaturesToDatabase { get; set; } = true;

    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsDynamicFeatureStoreEnabled { get; set; }
}
