using Secyud.Abp.AspNetCore.Components.Navigations;

namespace Secyud.Abp.Tenants.Navigation;

public class TenantsMenuNames
{
    public static MenuItem Group { get; } = new("Tenants")
    {
        CssClass = "fa fa-users"
    };
    public const string GroupUrl = "/tenants";

    public static MenuItem Tenants { get; } = new("Tenants", null, Group)
    {
        RequiredPermissions = [TenantsPermissions.Tenants.DefaultName],
        Url = TenantsUri
    };

    public const string TenantsUri = GroupUrl + "/tenants";
}