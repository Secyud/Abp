using Volo.Abp;

namespace Secyud.Abp.GlobalFeatures;

public class GlobalModuleFeaturesDictionary(GlobalFeatureManager featureManager)
    : Dictionary<string, GlobalModuleFeatures>
{
    public GlobalFeatureManager FeatureManager { get; } = Check.NotNull(featureManager, nameof(featureManager));
}
