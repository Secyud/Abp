using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions;

public class Permission<TResource>(
    string permissionName,
    string? displayName = null,
    Permission<TResource>? parentPermission = null) : IPermission
{
    public string Name { get; } = parentPermission is null ? permissionName : $"{parentPermission.Name}.{permissionName}";
    public ILocalizableString LocalizableString { get; } = L($"Permission:{displayName ?? permissionName}");
    public MultiTenancySides MultiTenancySides { get; init; }
    public bool IsEnabled { get; init; }

    private static ILocalizableString L(string name)
    {
        return Volo.Abp.Localization.LocalizableString.Create<TResource>(name);
    }

    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string Import = "Import";
    public const string Export = "Export";
}