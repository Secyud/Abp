using System.Reflection;
using Secyud.Secits.Blazor.Options;

namespace Secyud.Abp.AspNetCore.Theming;

[AttributeUsage(AttributeTargets.Class)]
public class ThemeColorAttribute(UiThemeColor color) : Attribute
{
    public UiThemeColor Color { get; set; } = color;

    public static UiThemeColor GetColor(Type themeType)
    {
        return themeType
            .GetCustomAttribute<ThemeColorAttribute>(true)
            ?.Color ?? UiThemeColor.Default;
    }
}