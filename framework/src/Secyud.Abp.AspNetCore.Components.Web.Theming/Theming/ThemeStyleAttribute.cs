using System.Reflection;
using Secyud.Secits.Blazor.Options;

namespace Secyud.Abp.AspNetCore.Theming;

[AttributeUsage(AttributeTargets.Class)]
public class ThemeStyleAttribute(UiThemeStyle style) : Attribute
{
    public UiThemeStyle Style { get; set; } = style;

    public static UiThemeStyle GetStyle(Type themeType)
    {
        return themeType
            .GetCustomAttribute<ThemeStyleAttribute>(true)
            ?.Style ?? UiThemeStyle.Default;
    }
}