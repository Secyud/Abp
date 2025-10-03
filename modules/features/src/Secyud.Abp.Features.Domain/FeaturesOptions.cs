using Volo.Abp.Collections;

namespace Secyud.Abp.Features;

public class FeaturesOptions
{
    public ITypeList<IFeaturesProvider> Providers { get; }

    public Dictionary<string, string> ProviderPolicies { get; }

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveStaticFeaturesToDatabase { get; set; } = true;

    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsDynamicFeatureStoreEnabled { get; set; }

    public FeaturesOptions()
    {
        Providers = new TypeList<IFeaturesProvider>();
        ProviderPolicies = new Dictionary<string, string>();
    }
}
