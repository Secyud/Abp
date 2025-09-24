using Secyud.Abp.AspNetCore.Components;
using Secyud.Abp.AspNetCore.Components.Theming;
using Secyud.Abp.AspNetCore.Navigation;

namespace Secyud.Abp.AspNetCore.Styles;

public class SecitsTheme : ITheme
{
    public const string Name = "Default";
    
    public Type GetLayout(string name, bool fallbackToDefault = true)
    {
        return typeof(SecitsPageRouteTabsLayout);
    }
}
