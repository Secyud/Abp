using System.Collections.Immutable;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionDefinition<TResource> : IPermissionDefinition
{
    private readonly List<PermissionDefinition<TResource>> _children = [];
    private readonly string _permissionName;
    private readonly string? _displayName;
    private readonly PermissionGroupDefinition<TResource> _group;
    public string Name { get; }
    public string? ParentName => Parent?.Name;
    public PermissionDefinition<TResource>? Parent { get; }

    public MultiTenancySides MultiTenancySide { get; set; }

    public ILocalizableString DisplayName =>
        field ??= LocalizableString.Create<TResource>($"Permission:{_displayName ?? _permissionName}");

    public IReadOnlyList<IPermissionDefinition> Children => _children.ToImmutableList();

    public List<string> Providers { get; } = [];

    public List<ISimpleStateChecker<IPermissionDefinition>> StateCheckers { get; } = [];

    public Dictionary<string, object?> Properties { get; } = [];
    public IPermissionGroupDefinition Group => _group;

    public PermissionDefinition(string permissionName,
        PermissionDefinition<TResource> parent,
        string? displayName = null)
    {
        _permissionName = permissionName;
        _displayName = displayName;
        _group = parent._group;
        Name = $"{parent.Name}.{permissionName}";
        Parent = parent;
        Parent._children.Add(this);
    }

    public PermissionDefinition(string permissionName,
        PermissionGroupDefinition<TResource> group,
        string? displayName = null)
    {
        _permissionName = permissionName;
        _displayName = displayName;
        _group = group;
        _group.AddPermission(this);

        Name = $"{group.Name}.{permissionName}";
    }

    public object? this[string name]
    {
        get => Properties.GetValueOrDefault(name);
        set => Properties[name] = value;
    }

    public virtual PermissionDefinition<TResource> WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    public virtual PermissionDefinition<TResource> WithProvider(string providerName)
    {
        Providers.AddIfNotContains(providerName);
        return this;
    }

    public void RemoveSelf()
    {
        Parent?._children.Remove(this);
        RemoveFromGroup();
    }

    private void RemoveFromGroup()
    {
        _group.RemovePermission(this);
        foreach (var child in _children)
        {
            child.RemoveFromGroup();
        }
    }

    public override string ToString()
    {
        return $"[{nameof(PermissionDefinition<>)} {Name}]";
    }
}