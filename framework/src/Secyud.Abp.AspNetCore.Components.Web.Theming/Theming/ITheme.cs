namespace Secyud.Abp.AspNetCore.Components.Theming;

public interface ITheme
{
    Type GetLayout(string name, bool fallbackToDefault = true);
}
