using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using Secyud.Secits.Blazor;
using Secyud.Secits.Blazor.Icons;
using Secyud.Secits.Blazor.PageRouters;

namespace Secyud.Abp.AspNetCore.Navigation;

public partial class TabRouterView
{
    protected TabContainer? TabContainer { get; set; }


    [Inject]
    protected PageRouterManager PageRouterManager { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IIconProvider IconProvider { get; set; } = null!;

    [CascadingParameter]
    public RouteData? RouteData { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        PageRouterManager.StateChanged += ViewStateChanged;
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            OnLocationChanged(null, new LocationChangedEventArgs("", true));
        }
    }

    protected void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        if (RouteData is not null)
            PageRouterManager.ActivatePageRouteItem(RouteData, NavigationManager.Uri);
    }

    protected async void ViewStateChanged(object? sender, EventArgs args)
    {
        try
        {
            await InvokeAsync(StateHasChanged);
            if (TabContainer is not null)
                await TabContainer.SelectTabAsync(
                    PageRouterManager.CurrentItem?.Id);
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected void CloseTabAsync(PageRouterItem item)
    {
        PageRouterManager.RemovePageRouteItem(item);
    }
    
    protected Func<string> CreateDisplayNameGetter(PageRouterItem item)
    {
        var resourceType = item.ResourceType ?? typeof(AbpUiResource);
        var lazy = new Lazy<IStringLocalizer>(() => StringLocalizerFactory.Create(resourceType));
        return () =>
        {
            var l = lazy.Value;
            return item.Name is null ? l["NewPage"] : l[item.Name, item.Parameters.Select(u => l[u])];
        };
    }

    protected override void Dispose(bool disposing)
    {
        PageRouterManager.StateChanged -= ViewStateChanged;
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}