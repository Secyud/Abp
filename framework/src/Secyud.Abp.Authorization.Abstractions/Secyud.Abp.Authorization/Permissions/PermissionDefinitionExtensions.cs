using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Authorization.Permissions;

public static class PermissionDefinitionExtensions
{
    internal static string CreateLocalizableStringKey(string permissionName, bool split = true)
    {
        if (split)
        {
            var index = permissionName.LastIndexOf('.');
            if (index >= 0)
            {
                return permissionName[(index + 1)..];
            }
        }

        return $"Permission:{permissionName}";
    }

    extension(IWithPermissions withPermissions)
    {
        public PermissionDefinition AddPermission<TResource>(
            string permissionName, string? displayName = null,
            MultiTenancySides multiTenancySide = MultiTenancySides.Both)
        {
            return withPermissions.AddPermission(permissionName,
                LocalizableString.Create<TResource>(
                    CreateLocalizableStringKey(displayName ?? permissionName, split: displayName is null)),
                multiTenancySide);
        }
    }

    extension(PermissionDefinitionContext context)
    {
        public PermissionGroupDefinition AddGroup(
            string groupName, ILocalizableString? displayName = null)
        {
            var group = new PermissionGroupDefinition(groupName)
            {
                DisplayName =
                    displayName ?? new FixedLocalizableString(
                        CreateLocalizableStringKey(groupName, false))
            };
            context.AddGroup(group);
            return group;
        }

        public PermissionGroupDefinition AddGroup<TResource>(
            string groupName, string? displayName = null)
        {
            return context.AddGroup(groupName,
                LocalizableString.Create<TResource>(
                    CreateLocalizableStringKey(displayName ?? groupName, false)));
        }
    }
}