using System.Collections.Immutable;
using Volo.Abp.Localization;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionGroupDefinition<TResource>(
    string name,
    string? displayName) : IPermissionGroupDefinition
{
    private readonly List<IPermissionDefinition> _permissions = [];

    /// <summary>
    /// Unique name of the group.
    /// </summary>
    public string Name { get; } = name;

    public ILocalizableString DisplayName =>
        field ??= LocalizableString.Create<TResource>($"Permission:{displayName ?? Name}");

    public IReadOnlyList<IPermissionDefinition> Permissions => _permissions.ToImmutableList();
    public Dictionary<string, object?> Properties { get; } = [];

    internal void AddPermission(PermissionDefinition<TResource> permission)
    {
        _permissions.Add(permission);
    }

    internal void RemovePermission(PermissionDefinition<TResource> permission)
    {
        _permissions.Remove(permission);
    }
}