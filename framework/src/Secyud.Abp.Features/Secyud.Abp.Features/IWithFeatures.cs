using Volo.Abp.Localization;

namespace Secyud.Abp.Features;

public interface IWithFeatures
{
    FeatureDefinition AddFeature(string featureName, ILocalizableString? displayName = null);

    bool RemoveFeature(string featureName, bool recurse = false);
}