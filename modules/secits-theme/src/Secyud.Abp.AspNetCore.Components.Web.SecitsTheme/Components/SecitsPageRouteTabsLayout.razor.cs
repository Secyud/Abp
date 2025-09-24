using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Secyud.Secits.Blazor.PageRoutes;

namespace Secyud.Abp.AspNetCore.Components;

public sealed partial class SecitsPageRouteTabsLayout
{
    [Inject]
    private IStringLocalizerFactory Factory { get; set; } = null!;


    protected override Func<string> CreateDisplayNameGetter(PageRouteItem item)
    {
        var resourceType = item.ResourceType ?? typeof(AbpUiResource);
        var lazy = new Lazy<IStringLocalizer>(() => Factory.Create(resourceType));
        return () =>
        {
            var l = lazy.Value;
            return item.Name is null ? l["NewPage"] : l[item.Name, item.Parameters.Select(u => l[u])];
        };
    }

    protected override string GetClass()
    {
        return "sc-container scrollable " + base.GetClass();
    }
}