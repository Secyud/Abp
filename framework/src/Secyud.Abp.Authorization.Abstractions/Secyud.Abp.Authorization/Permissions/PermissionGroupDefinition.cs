using System.Collections.Immutable;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionGroupDefinition(string name) : IWithPermissions
{
    private readonly List<PermissionDefinition> _permissions = [];

    /// <summary>
    /// Unique name of the group.
    /// </summary>
    public string Name { get; } = name;

    public required ILocalizableString DisplayName { get; set; }

    public IReadOnlyList<PermissionDefinition> Permissions => _permissions.ToImmutableList();

    public Dictionary<string, object?> Properties { get; } = [];

    public object? this[string name]
    {
        get => Properties.GetValueOrDefault(name);
        set => Properties[name] = value;
    }

    public virtual PermissionGroupDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    public PermissionDefinition AddPermission(string permissionName, ILocalizableString? displayName = null,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both)
    {
        var permission = new PermissionDefinition(permissionName, this, null)
        {
            DisplayName = displayName ?? new FixedLocalizableString(
                PermissionDefinitionExtensions.CreateLocalizableStringKey(permissionName)),
            MultiTenancySide = multiTenancySide,
        };
        
        _permissions.Add(permission);
        return permission;
    }

    public bool RemovePermission(string permissionName, bool recurse)
    {
        var index = _permissions.FindIndex(u => u.Name == permissionName);
        if (index >= 0)
        {
            _permissions.RemoveAt(index);
            return true;
        }

        if (recurse)
        {
            foreach (var permission in _permissions)
            {
                if (permission.RemovePermission(permissionName, true))
                {
                    return true;
                }
            }
        }

        return false;
    }
}