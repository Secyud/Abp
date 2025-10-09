using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions;

public interface IPermission
{
    string Name { get; }
    ILocalizableString LocalizableString { get; }
    MultiTenancySides MultiTenancySides { get; }
    bool IsEnabled { get; }
}