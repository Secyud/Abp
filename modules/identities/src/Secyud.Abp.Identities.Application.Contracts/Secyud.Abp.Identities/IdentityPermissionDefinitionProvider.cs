using Volo.Abp.Authorization.Permissions;

namespace Secyud.Abp.Identities;

public class IdentityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var identityGroup = context.AddGroup(IdentityPermissions.Group);

        var rolesPermission = identityGroup.AddPermission(IdentityPermissions.Roles.Default);
        rolesPermission.AddChild(IdentityPermissions.Roles.Create);
        rolesPermission.AddChild(IdentityPermissions.Roles.Update);
        rolesPermission.AddChild(IdentityPermissions.Roles.Delete);
        rolesPermission.AddChild(IdentityPermissions.Roles.ManagePermissions);
        rolesPermission.AddChild(IdentityPermissions.Roles.ViewChangeHistory);

        var usersPermission = identityGroup.AddPermission(IdentityPermissions.Users.Default);
        usersPermission.AddChild(IdentityPermissions.Users.Create);
        var editPermission = usersPermission.AddChild(IdentityPermissions.Users.Update);
        editPermission.AddChild(IdentityPermissions.Users.ManageRoles);
        usersPermission.AddChild(IdentityPermissions.Users.Delete);
        usersPermission.AddChild(IdentityPermissions.Users.ManagePermissions);
        usersPermission.AddChild(IdentityPermissions.Users.ViewChangeHistory);
        usersPermission.AddChild(IdentityPermissions.Users.Impersonation);
        usersPermission.AddChild(IdentityPermissions.Users.Import);
        usersPermission.AddChild(IdentityPermissions.Users.Export);
        usersPermission.AddChild(IdentityPermissions.Users.ViewDetails);

        var claimTypesPermission = identityGroup.AddPermission(IdentityPermissions.ClaimTypes.Default);
        claimTypesPermission.AddChild(IdentityPermissions.ClaimTypes.Create);
        claimTypesPermission.AddChild(IdentityPermissions.ClaimTypes.Update);
        claimTypesPermission.AddChild(IdentityPermissions.ClaimTypes.Delete);

        identityGroup.AddPermission(IdentityPermissions.Settings.Default);

        identityGroup
            .AddPermission(IdentityPermissions.UserLookup.Default)
            .WithProviders(ClientPermissionValueProvider.ProviderName);

        var securityLogPermission = identityGroup.AddPermission(IdentityPermissions.SecurityLogs.Default);
        var sessionPermission = identityGroup.AddPermission(IdentityPermissions.Sessions.Default);
    }
}