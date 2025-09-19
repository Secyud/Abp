using Secyud.Secits.Blazor.Options;
using Volo.Abp;

namespace Secyud.Abp.AspNetCore.Components.Theming;

public class ThemeInfo
{
    public Type ThemeType { get; }
    public string Name { get; }

    public ThemeInfo(Type themeType)
    {
        Check.NotNull(themeType, nameof(themeType));

        if (!typeof(ITheme).IsAssignableFrom(themeType))
        {
            throw new AbpException($"Given {nameof(themeType)} ({themeType.AssemblyQualifiedName}) should be type of {typeof(ITheme).AssemblyQualifiedName}");
        }

        ThemeType = themeType;
        Name = ThemeNameAttribute.GetName(themeType);
    }
}