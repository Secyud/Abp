using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Secyud.Secits.Blazor.PageRoutes;
using Volo.Abp.AspNetCore.Components.Web.Security;

namespace Secyud.Abp.Secits.AspNetCore.Components.Navigation;

public partial class MainMenu
{
    [Inject] protected MainMenuProvider MainMenuProvider { get; set; } = null!;

    [Inject] protected PageRouteManager PageRouteManager { get; set; } = null!;

    [Inject]
    protected ApplicationConfigurationChangedService ApplicationConfigurationChangedService { get; set; } = null!;

    protected MenuViewModel? Menu { get; set; }

    private void MenuStateChanged(object? sender, EventArgs e)
    {
        if (Menu is null) return;
        var currentItem = PageRouteManager.CurrentItem;

        var menuItem = FindMenuItemByRouterItem(currentItem);

        if (menuItem is null)
        {
            Menu.DeactivateAll();
        }
        else if (currentItem is not null)
        {
            if (menuItem.DisplayName != currentItem.DisplayName)
            {
                currentItem.DisplayNameGetter = () => menuItem.DisplayName;
            }

            //Menu.CloseAll();
            Menu.Activate(menuItem);
        }

        InvokeAsync(StateHasChanged);
    }

    protected override async Task OnInitializedAsync()
    {
        Menu = await MainMenuProvider.GetMenuAsync();
        ApplicationConfigurationChangedService.Changed +=
            ApplicationConfigurationChanged;
        PageRouteManager.RouterItemActivated += MenuStateChanged;
    }

    private async void ApplicationConfigurationChanged()
    {
        try
        {
            Menu = await MainMenuProvider.GetMenuAsync();
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception e)
        {
            await HandleErrorAsync(e);
        }
    }

    protected virtual MenuItemViewModel? FindMenuItemByRouterItem(PageRouteItem? routerItem)
    {
        if (routerItem is null || Menu is null) return null;

        MenuItemViewModel? menuItem = null;

        if (routerItem.Key is null)
        {
            var uri = routerItem.Uri;
            var baseUri = new Uri($"{uri.Scheme}://{uri.Authority}");
            var model = Menu.LeafItems.FirstOrDefault(CompareUri);

            if (model is null)
            {
                routerItem.Key = "#";
            }
            else
            {
                menuItem = model;
                routerItem.Key = menuItem.Name;
            }

            bool CompareUri(MenuItemViewModel item)
            {
                var uriStr = item.Url;
                if (uriStr is null) return false;
                var menuUri = uriStr.StartsWith("http")
                    ? new Uri(uriStr)
                    : new Uri(baseUri, uriStr);
                return 0 == Uri.Compare(menuUri, uri, UriComponents.PathAndQuery, UriFormat.UriEscaped,
                    StringComparison.InvariantCultureIgnoreCase);
            }
        }
        else if (routerItem.Key != "#")
        {
            menuItem = Menu.LeafItems.FirstOrDefault(u => u.Name == routerItem.Key);
        }

        return menuItem;
    }

    protected virtual async Task ToggleMenuAsync(MenuItemViewModel model)
    {
        if (Menu is not null)
        {
            Menu.ToggleOpen(model);
            await InvokeAsync(StateHasChanged);
        }
    }

    protected override void Dispose(bool disposing)
    {
        PageRouteManager.RouterItemActivated -= MenuStateChanged;
        ApplicationConfigurationChangedService.Changed -= ApplicationConfigurationChanged;
    }
}