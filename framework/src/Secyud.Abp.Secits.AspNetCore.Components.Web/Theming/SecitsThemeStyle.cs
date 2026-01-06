using Secyud.Secits.Blazor;
using Volo.Abp.Localization;

namespace Secyud.Abp.Secits.AspNetCore.Components.Theming;

public class SecitsThemeStyle(LocalizableString displayName, string? icon = null) : SecitsThemeInput
{
    public LocalizableString DisplayName { get; set; } = displayName;
    public string? Icon { get; set; } = icon;
    public int Order { get; set; }
}