using Secyud.Abp.AspNetCore.Components;
using Secyud.Abp.AspNetCore.Styles;
using Secyud.Secits.Blazor;
using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore;

public class SecitsThemeOptions
{
    public Dictionary<string, SecitsThemeStyle> Styles { get; } = new();

    /// <summary>
    /// Defines the default fallback theme. Default value is <see cref="SecitsStyleNames.System"/>
    /// </summary>
    public string DefaultStyle { get; set; } = SecitsStyleNames.System;

    public Type Layout { get; set; } = typeof(SecitsPageRouteTabsLayout);

    public SecitsThemeStyle GetDefaultStyle()
    {
        if (string.IsNullOrEmpty(DefaultStyle) || !Styles.TryGetValue(DefaultStyle, out var style))
        {
            return Styles.FirstOrDefault().Value;
        }

        return style;
    }

    public SecitsThemeInput GenerateThemeInput(string styleName)
    {
        var option = new SecitsThemeInput();

        if (Styles.GetValueOrDefault(styleName) is { } style)
            foreach (var (key, value) in style.Parameters)
                option.Parameters[key] = value;
        
        option.IsRtl = CultureHelper.IsRtl;
        return option;
    }
}