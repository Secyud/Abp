namespace Secyud.Abp.AspNetCore.Styles;

public interface ISecitsStyleProvider
{
    event EventHandler? StyleChanged;

    Task<string> GetCurrentStyleAsync();

    Task SetCurrentStyleAsync(string styleName);
}