using Volo.Abp.Localization;

namespace Secyud.Abp.Authorization.Permissions;

public interface IPermissionGroupDefinition
{
    /// <summary>
    /// Unique name of the permission.
    /// </summary>
    public string Name { get; }

    public ILocalizableString DisplayName { get; }

    public IReadOnlyList<IPermissionDefinition> Permissions { get; }

    public Dictionary<string, object?> Properties { get; }
}