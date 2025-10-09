using Secyud.Abp.Settings.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Secyud.Abp.Settings;

public class SettingsFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(SettingsFeatures.GroupName,
            L("Feature:SettingsGroup"));

        var settingEnableFeature = group.AddFeature(
            SettingsFeatures.Enable,
            "true",
            L("Feature:SettingsEnable"),
            L("Feature:SettingsEnableDescription"),
            new ToggleStringValueType(),
            isAvailableToHost: false);

        settingEnableFeature.CreateChild(
            SettingsFeatures.AllowChangingEmailSettings,
            "false",
            L("Feature:AllowChangingEmailSettings"),
            null,
            new ToggleStringValueType(),
            isAvailableToHost: false);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSettingsResource>(name);
    }
}
