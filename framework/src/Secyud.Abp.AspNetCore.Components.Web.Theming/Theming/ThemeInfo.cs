using Secyud.Secits.Blazor.Options;
using Volo.Abp;

namespace Secyud.Abp.AspNetCore.Theming;

public class ThemeInfo
{
    public Type ThemeType { get; }
    public string Name { get; }
    public UiThemeColor Color { get; set; }
    public UiThemeParam Param { get; set; }
    public UiThemeStyle Style { get; set; }

    public ThemeInfo(Type themeType)
    {
        Check.NotNull(themeType, nameof(themeType));

        if (!typeof(ITheme).IsAssignableFrom(themeType))
        {
            throw new AbpException($"Given {nameof(themeType)} ({themeType.AssemblyQualifiedName}) should be type of {typeof(ITheme).AssemblyQualifiedName}");
        }

        ThemeType = themeType;
        Name = ThemeNameAttribute.GetName(themeType);
        Color = ThemeColorAttribute.GetColor(themeType);
        Param = ThemeParamAttribute.GetParam(themeType);
        Style = ThemeStyleAttribute.GetStyle(themeType);
    }
}