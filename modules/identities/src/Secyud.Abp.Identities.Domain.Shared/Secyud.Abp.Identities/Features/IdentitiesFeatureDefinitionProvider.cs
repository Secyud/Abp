using Secyud.Abp.Identities.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Secyud.Abp.Identities.Features;

public class IdentitiesFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(IdentitiesFeature.GroupName, L("Feature:IdentityGroup"));

        group.AddFeature(IdentitiesFeature.TwoFactor,
            nameof(IdentitiesTwoFactorBehaviour.Optional),
            L("Feature:TwoFactor"),
            L("Feature:TwoFactorDescription"),
            new SelectionStringValueType
            {
                ItemSource = new StaticSelectionStringValueItemSource(
                    new LocalizableSelectionStringValueItem
                    {
                        Value = nameof(IdentitiesTwoFactorBehaviour.Optional),
                        DisplayText = GetTwoFactorBehaviourLocalizableStringInfo("Feature:TwoFactor.Optional")
                    },
                    new LocalizableSelectionStringValueItem
                    {
                        Value = nameof(IdentitiesTwoFactorBehaviour.Disabled),
                        DisplayText = GetTwoFactorBehaviourLocalizableStringInfo("Feature:TwoFactor.Disabled")
                    },
                    new LocalizableSelectionStringValueItem
                    {
                        Value = nameof(IdentitiesTwoFactorBehaviour.Forced),
                        DisplayText = GetTwoFactorBehaviourLocalizableStringInfo("Feature:TwoFactor.Forced")
                    }
                )
            });

        group.AddFeature(IdentitiesFeature.MaxUserCount,
            "0", //0 = unlimited
            L("Feature:MaximumUserCount"),
            L("Feature:MaximumUserCountDescription"),
            new FreeTextStringValueType(new NumericValueValidator(0)));

        group.AddFeature(IdentitiesFeature.EnableLdapLogin,
            false.ToString(),
            L("Feature:EnableLdapLogin"),
            null,
            new ToggleStringValueType());
        
        group.AddFeature(IdentitiesFeature.EnableOAuthLogin,
            false.ToString(),
            L("Feature:EnableOAuthLogin"),
            null,
            new ToggleStringValueType());
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpIdentitiesResource>(name);
    }

    private static LocalizableStringInfo GetTwoFactorBehaviourLocalizableStringInfo(string key)
    {
        return new LocalizableStringInfo(LocalizationResourceNameAttribute.GetName(typeof(AbpIdentitiesResource)), key);
    }
}
