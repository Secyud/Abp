using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Authorization.Permissions;

public interface IPermissionDefinition : IHasSimpleStateCheckers<IPermissionDefinition>
{
    /// <summary>
    /// Unique name of the permission.
    /// </summary>
    public string Name { get; }

    public string? ParentName { get; }

    public MultiTenancySides MultiTenancySide { get; }

    public ILocalizableString DisplayName { get; }

    public IReadOnlyList<IPermissionDefinition> Children { get; }

    public List<string> Providers { get; }

    public Dictionary<string, object?> Properties { get; }
    public IPermissionGroupDefinition Group { get; }
}