namespace Secyud.Abp.AspNetCore.Theming;

public interface ITheme
{
    Type GetLayout(string name, bool fallbackToDefault = true);
}
