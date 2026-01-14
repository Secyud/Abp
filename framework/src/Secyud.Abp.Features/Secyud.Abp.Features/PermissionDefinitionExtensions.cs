using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Features;

public static class FeatureDefinitionExtensions
{
    internal static string CreateLocalizableStringKey(string featureName, bool split = true)
    {
        if (split)
        {
            var index = featureName.LastIndexOf('.');
            if (index >= 0)
            {
                return featureName[(index + 1)..];
            }
        }

        return $"Feature:{featureName}";
    }

    extension(IWithFeatures withFeatures)
    {
        public FeatureDefinition AddFeature<TResource>(
            string featureName, string? displayName = null)
        {
            return withFeatures.AddFeature(featureName,
                LocalizableString.Create<TResource>(
                    CreateLocalizableStringKey(displayName ?? featureName, split: displayName is null)));
        }
    }

    extension(FeatureDefinitionContext context)
    {
        public FeatureGroupDefinition AddGroup(
            string groupName, ILocalizableString? displayName = null)
        {
            var group = new FeatureGroupDefinition(groupName)
            {
                DisplayName =
                    displayName ?? new FixedLocalizableString(
                        CreateLocalizableStringKey(groupName, false))
            };
            context.AddGroup(group);
            return group;
        }

        public FeatureGroupDefinition AddGroup<TResource>(
            string groupName, string? displayName = null)
        {
            return context.AddGroup(groupName,
                LocalizableString.Create<TResource>(
                    CreateLocalizableStringKey(displayName ?? groupName, false)));
        }
    }
}