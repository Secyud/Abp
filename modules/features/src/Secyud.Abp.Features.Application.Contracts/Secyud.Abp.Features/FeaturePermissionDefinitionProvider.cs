using Secyud.Abp.Features.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Features;

public class FeaturePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var featuresGroup = context.AddGroup(
            FeaturesPermissions.GroupName,
            L("Permission:Features"));

        featuresGroup.AddPermission(
            FeaturesPermissions.ManageHostFeatures,
            L("Permission:Features.ManageHostFeatures"),
            multiTenancySide: MultiTenancySides.Host);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpFeaturesResource>(name);
    }
}
