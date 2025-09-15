namespace Secyud.Abp.AspNetCore.Theming;

public class AbpThemingOptions
{
    public ThemeDictionary Themes { get; } = new();

    public string? DefaultThemeName { get; set; }
}
