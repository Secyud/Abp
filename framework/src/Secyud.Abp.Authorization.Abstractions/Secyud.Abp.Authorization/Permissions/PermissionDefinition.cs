using System.Collections.Immutable;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionDefinition
    : IWithPermissions, IHasSimpleStateCheckers<PermissionDefinition>
{
    private readonly List<PermissionDefinition> _children = [];

    internal PermissionDefinition(string name, PermissionGroupDefinition group, PermissionDefinition? parent)
    {
        Name = name;
        Group = group;
        Parent = parent;
    }

    public PermissionGroupDefinition Group { get; }
    public PermissionDefinition? Parent { get; }
    public string Name { get; }
    public required ILocalizableString DisplayName { get; set; }
    public required MultiTenancySides MultiTenancySide { get; set; }
    public bool IsDisabled { get; set; }
    public List<string> Providers { get; } = [];
    public List<ISimpleStateChecker<PermissionDefinition>> StateCheckers { get; } = [];
    public IReadOnlyList<PermissionDefinition> Children => _children.ToImmutableList();

    public Dictionary<string, object?> Properties { get; } = [];

    public object? this[string name]
    {
        get => Properties.GetValueOrDefault(name);
        set => Properties[name] = value;
    }

    public virtual PermissionDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    public virtual PermissionDefinition WithProvider(string providerName)
    {
        Providers.AddIfNotContains(providerName);
        return this;
    }

    public virtual PermissionDefinition WithProviders(params string[] providerNames)
    {
        Providers.AddIfNotContains(providerNames);
        return this;
    }

    public override string ToString()
    {
        return $"[{nameof(PermissionDefinition)} {Name}]";
    }

    public PermissionDefinition AddPermission(string permissionName, ILocalizableString? displayName = null,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both)
    {
        var permission = new PermissionDefinition(permissionName, Group, this)
        {
            DisplayName = displayName ?? new FixedLocalizableString(
                PermissionDefinitionExtensions.CreateLocalizableStringKey(permissionName)),
            MultiTenancySide = multiTenancySide,
        };
        _children.Add(permission);
        return permission;
    }

    public bool RemovePermission(string permissionName, bool recurse)
    {
        if (!recurse)
        {
            var index = _children.FindIndex(x => x.Name == permissionName);
            if (index < 0) return false;
            _children.RemoveAt(index);
            return true;
        }
        
        
        var queue = new Queue<List<PermissionDefinition>>();
        queue.Enqueue(_children);

        while (queue.Count > 0)
        {
            var list = queue.Dequeue();

            for (var i = 0; i < list.Count; i++)
            {
                var permission = list[i];
                if (permission.Name == permissionName)
                {
                    list.RemoveAt(i);
                    return true;
                }

                if (permission._children.Count > 0)
                {
                    queue.Enqueue(list);
                }
            }
        }

        return false;
    }

    public PermissionDefinition SetDisabled(bool isDisabled = true)
    {
        IsDisabled = isDisabled;
        return this;
    }
}