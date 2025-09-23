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
    protected STabContainer? TabContainer { get; set; }

    [Inject]
    protected PageRouterManager PageRouterManager { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IIconProvider IconProvider { get; set; } = null!;

    [CascadingParameter]
    public RouteData? RouteData { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
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
        {
            PageRouterManager.ActivatePageRouteItem(RouteData, NavigationManager.Uri);
            OnActivatedRouteItem().ConfigureAwait(false);
        }
    }

    protected async Task OnActivatedRouteItem()
    {
        if (TabContainer is not null)
        {
            await TabContainer.SelectTabAsync(PageRouterManager.CurrentItem?.Id);
            await InvokeAsync(StateHasChanged);
        }
    }

    protected void CloseTabAsync(PageRouterItem item)
    {
        PageRouterManager.RemovePageRouteItem(item);
        NavigationManager.NavigateTo(PageRouterManager.CurrentItem?.Uri.ToString() ?? "/");
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

    protected void NavigateTo(Uri uri)
    {
        NavigationManager.NavigateTo(uri.AbsolutePath);
    }

    protected override void Dispose(bool disposing)
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    protected void OnTabOptioned(Tab<PageRouterItem> tab)
    {
        tab.Key = tab.Item.Id;
        tab.PreventDefaultClick = true;
        tab.Index = tab.Item.Sequence;
    }
}