namespace Secyud.Abp.Secits.AspNetCore.Components.Theming;

public interface ISecitsThemeManager
{
    Task<string> GetCurrentThemeAsync();
    Task SetCurrentThemeAsync(string style);
}