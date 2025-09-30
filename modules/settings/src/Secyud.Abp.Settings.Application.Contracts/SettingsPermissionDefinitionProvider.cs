using Secyud.Abp.Settings.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Secyud.Abp.Settings;

public class SettingsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var moduleGroup = context.AddGroup(SettingsPermissions.GroupName, L("Permission:Settings"));

        var emailPermission = moduleGroup
            .AddPermission(SettingsPermissions.Emailing, L("Permission:Emailing"));
        emailPermission.StateCheckers.Add(new AllowChangingEmailSettingsFeatureSimpleStateChecker());

        emailPermission.AddChild(SettingsPermissions.EmailingTest, L("Permission:EmailingTest"));

        moduleGroup.AddPermission(SettingsPermissions.TimeZone, L("Permission:TimeZone"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpSettingsResource>(name);
    }
}
