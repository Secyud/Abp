using Secyud.Secits.Blazor;
using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore.Styles;

public class SecitsThemeStyle(LocalizableString displayName, string? icon = null) : SecitsThemeParam
{
    public LocalizableString DisplayName { get; set; } = displayName;

    public string? Icon { get; set; } = icon;
}