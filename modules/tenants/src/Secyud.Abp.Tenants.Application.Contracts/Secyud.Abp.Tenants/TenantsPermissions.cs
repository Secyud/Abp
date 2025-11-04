using Secyud.Abp.Tenants.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Tenants;

public static class TenantsPermissions
{
    public class TenantsPermission(string permissionName, string? displayName = null, Permission<AbpTenantsResource>? parentPermission = null)
        : Permission<AbpTenantsResource>(permissionName, displayName, parentPermission);

    public const string GroupName = "AbpTenants";
    public static TenantsPermission Group { get; } = new(GroupName);


    public static class Tenants
    {
        private const string Name = GroupName + "Tenants";

        public static TenantsPermission Default { get; } = new(Name, null, Group)
        {
            MultiTenancySides = MultiTenancySides.Host
        };

        public const string DefaultName = $"{GroupName}.{Name}";

        public static TenantsPermission Create { get; } = new(TenantsPermission.Create, null, Default)
        {
            MultiTenancySides = MultiTenancySides.Host
        };

        public const string CreateName = $"{GroupName}.{TenantsPermission.Create}";

        public static TenantsPermission Update { get; } = new(TenantsPermission.Update, null, Default)
        {
            MultiTenancySides = MultiTenancySides.Host
        };

        public const string UpdateName = $"{GroupName}.{TenantsPermission.Update}";

        public static TenantsPermission Delete { get; } = new(TenantsPermission.Delete, null, Default)
        {
            MultiTenancySides = MultiTenancySides.Host
        };

        public const string DeleteName = $"{GroupName}.{TenantsPermission.Delete}";

        public static TenantsPermission ManageFeatures { get; } = new("ManageFeatures", null, Default)
        {
            MultiTenancySides = MultiTenancySides.Host
        };

        public static TenantsPermission ManageConnectionStrings { get; } = new(ManageConnectionStringsAlias, null, Default)
        {
            MultiTenancySides = MultiTenancySides.Host
        };

        private const string ManageConnectionStringsAlias = "ManageConnectionStrings";
        public const string ManageConnectionStringsName = $"{GroupName}.{ManageConnectionStringsAlias}";
    }
}