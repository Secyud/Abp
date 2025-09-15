using Secyud.Abp.AspNetCore.Components;
using Secyud.Abp.AspNetCore.Theming;

namespace Secyud.Abp.AspNetCore;

public class SecitsDefaultTheme : ITheme
{
    public const string Name = "Default";
    
    public Type GetLayout(string name, bool fallbackToDefault = true)
    {
        return typeof(MainLayout);
    }
}