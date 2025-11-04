using Secyud.Abp.AspNetCore.Components.Navigations;

namespace Secyud.Abp.Identities.Navigation;

public class IdentitiesMenus
{
    public static MenuItem Group { get; } = new("AbpIdentities")
    {
        Icon = "fa fa-id-card"
    };

    public const string GroupUrl = "/identities";

    public static MenuItem Roles { get; } = new("Roles", null, Group)
    {
        Url = RolesUrl,
        RequiredPermissions = [IdentityPermissions.Roles.DefaultName]
    };

    public const string RolesUrl = GroupUrl + "/roles";

    public static MenuItem Users { get; } = new("Users", null, Group)
    {
        Url = UsersUrl,
        RequiredPermissions = [IdentityPermissions.Users.DefaultName]
    };

    public const string UsersUrl = GroupUrl + "/users";

    public static MenuItem ClaimTypes { get; } = new("ClaimTypes", null, Group)
    {
        Url = ClaimTypesUrl,
        RequiredPermissions = [IdentityPermissions.ClaimTypes.DefaultName]
    };

    public const string ClaimTypesUrl = GroupUrl + "/claim-types";

    public static MenuItem SecurityLogs { get; } = new("SecurityLogs", null, Group)
    {
        Url = SecurityLogsUrl,
        RequiredPermissions = [IdentityPermissions.SecurityLogs.DefaultName]
    };

    public const string SecurityLogsUrl = GroupUrl + "/security-logs";

    public static MenuItem Settings { get; } = new("IdentitiesSettings");
}