using SecitsDemoApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace SecitsDemoApp.Permissions;

public class SecitsDemoAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SecitsDemoAppPermissions.GroupName, L("Permission:SecitsDemoApp"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SecitsDemoAppResource>(name);
    }
}
