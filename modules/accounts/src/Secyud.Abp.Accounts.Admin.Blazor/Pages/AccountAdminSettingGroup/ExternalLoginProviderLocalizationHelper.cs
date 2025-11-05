using JetBrains.Annotations;
using Microsoft.Extensions.Localization;

namespace Secyud.Abp.Accounts.Pages.AccountAdminSettingGroup;

public static class ExternalLoginProviderLocalizationHelper
{
    public static string Localize(
        IStringLocalizer? localizer,
        string key,
        string defaultValue)
    {
        if (localizer == null)
        {
            return defaultValue;
        }

        var result = localizer[key];
        if (result.ResourceNotFound)
        {
            return defaultValue;
        }

        return result.Value;
    }
}
