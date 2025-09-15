using System.Reflection;
using Secyud.Secits.Blazor.Options;

namespace Secyud.Abp.AspNetCore.Theming;

[AttributeUsage(AttributeTargets.Class)]
public class ThemeParamAttribute(UiThemeParam param) : Attribute
{
    public UiThemeParam Param { get; set; } = param;

    public static UiThemeParam GetParam(Type themeType)
    {
        return themeType
            .GetCustomAttribute<ThemeParamAttribute>(true)
            ?.Param ?? UiThemeParam.Default;
    }
}