using Secyud.Abp.Identities.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Identities;

public static class IdentityPermissions
{
    public class IdentitiesPermission(string permissionName, string? displayName = null, Permission<AbpIdentitiesResource>? parentPermission = null)
        : Permission<AbpIdentitiesResource>(permissionName, displayName, parentPermission);

    public const string GroupName = "AbpIdentity";
    public static IdentitiesPermission Group { get; } = new(GroupName);

    public static class Settings
    {
        private const string Name = "Settings";
        public static IdentitiesPermission Default { get; } = new(Name, null, Group);
        public const string DefaultName = $"{GroupName}.{Name}";
    }

    public static class Roles
    {
        private const string Name = "Roles";
        public static IdentitiesPermission Default { get; } = new(Name, null, Group);
        public const string DefaultName = $"{GroupName}.{Name}";
        public static IdentitiesPermission Create { get; } = new(IdentitiesPermission.Create, null, Default);
        public const string CreateName = $"{GroupName}.{IdentitiesPermission.Create}";
        public static IdentitiesPermission Update { get; } = new(IdentitiesPermission.Update, null, Default);
        public const string UpdateName = $"{GroupName}.{IdentitiesPermission.Update}";
        public static IdentitiesPermission Delete { get; } = new(IdentitiesPermission.Delete, null, Default);
        public const string DeleteName = $"{GroupName}.{IdentitiesPermission.Delete}";
        public static IdentitiesPermission ManagePermissions { get; } = new("ManagePermissions", null, Default);
        public static IdentitiesPermission ViewChangeHistory { get; } = new("ViewChangeHistory", null, Default);
    }

    public static class Users
    {
        private const string Name = "Users";
        public static IdentitiesPermission Default { get; } = new(Name, null, Group);
        public const string DefaultName = $"{GroupName}.{Name}";
        public static IdentitiesPermission Create { get; } = new(IdentitiesPermission.Create, null, Default);
        public const string CreateName = $"{GroupName}.{IdentitiesPermission.Create}";
        public static IdentitiesPermission Update { get; } = new(IdentitiesPermission.Update, null, Default);
        public const string UpdateName = $"{GroupName}.{IdentitiesPermission.Update}";
        public static IdentitiesPermission Delete { get; } = new(IdentitiesPermission.Delete, null, Default);
        public const string DeleteName = $"{GroupName}.{IdentitiesPermission.Delete}";
        public static IdentitiesPermission Import { get; } = new(IdentitiesPermission.Import, null, Default);
        public const string ImportName = $"{GroupName}.{IdentitiesPermission.Import}";
        public static IdentitiesPermission Export { get; } = new(IdentitiesPermission.Export, null, Default);
        public const string ExportName = $"{GroupName}.{IdentitiesPermission.Export}";
        public static IdentitiesPermission ViewDetails { get; } = new("ViewDetails", null, Default);
        public static IdentitiesPermission ManagePermissions { get; } = new("ManagePermissions", null, Default);
        public static IdentitiesPermission ViewChangeHistory { get; } = new("ViewChangeHistory", null, Default);
        public static IdentitiesPermission Impersonation { get; } = new("Impersonation", null, Default);
        public static IdentitiesPermission ManageRoles { get; } = new("ManageRoles", null, Update);
    }

    public static class ClaimTypes
    {
        private const string Name = "ClaimTypes";

        public static IdentitiesPermission Default { get; } = new(Name, null, Group)
        {
            MultiTenancySides = MultiTenancySides.Host
        };

        public const string DefaultName = $"{GroupName}.{Name}";

        public static IdentitiesPermission Create { get; } = new(IdentitiesPermission.Create, null, Default)
        {
            MultiTenancySides = MultiTenancySides.Host
        };

        public const string CreateName = $"{GroupName}.{IdentitiesPermission.Create}";

        public static IdentitiesPermission Update { get; } = new(IdentitiesPermission.Update, null, Default)
        {
            MultiTenancySides = MultiTenancySides.Host
        };

        public const string UpdateName = $"{GroupName}.{IdentitiesPermission.Update}";

        public static IdentitiesPermission Delete { get; } = new(IdentitiesPermission.Delete, null, Default)
        {
            MultiTenancySides = MultiTenancySides.Host
        };

        public const string DeleteName = $"{GroupName}.{IdentitiesPermission.Delete}";
    }

    public static class UserLookup
    {
        private const string Name = "UserLookup";
        public static IdentitiesPermission Default { get; } = new(Name, null, Group);
        public const string DefaultName = $"{GroupName}.{Name}";
    }

    public static class SecurityLogs
    {
        private const string Name = "SecurityLogs";
        public static IdentitiesPermission Default { get; } = new(Name, null, Group);
        public const string DefaultName = $"{GroupName}.{Name}";
    }

    public static class Sessions
    {
        private const string Name = "Sessions";
        public static IdentitiesPermission Default { get; } = new(Name, null, Group);
        public const string DefaultName = $"{GroupName}.{Name}";
    }
}