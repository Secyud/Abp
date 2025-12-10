using System.Globalization;
using Microsoft.Extensions.Localization;
using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore.Localization;

public class AppAbpDictionaryBasedStringLocalizer(
    LocalizationResourceBase resource,
    List<IStringLocalizer> baseLocalizers,
    AbpLocalizationOptions abpLocalizationOptions)
    : AbpDictionaryBasedStringLocalizer(resource, baseLocalizers, abpLocalizationOptions)
{
    protected virtual string CurrentCultureName =>
        CultureInfo.DefaultThreadCurrentUICulture?.Name ?? CultureInfo.CurrentUICulture.Name;
    
    protected override LocalizedString GetLocalizedString(string name)
    {
        return GetLocalizedString(name, CurrentCultureName);
    }

    protected override LocalizedString GetLocalizedStringFormatted(string name, params object[] arguments)
    {
        return GetLocalizedStringFormatted(name, CurrentCultureName, arguments);
    }
}