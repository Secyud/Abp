using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Secyud.Secits.Blazor.Icons;
using Secyud.Secits.Blazor.PageRoutes;
using Volo.Abp.AspNetCore.Components.Web.Security;

namespace Secyud.Abp.AspNetCore.Navigation;

public partial class MainMenu
{
    [Inject]
    protected MainMenuProvider MainMenuProvider { get; set; } = null!;

    [Inject]
    protected PageRouteManager PageRouteManager { get; set; } = null!;

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
            if (menuItem.MenuItem.DisplayName != currentItem.DisplayName)
            {
                currentItem.DisplayNameGetter = () => menuItem.MenuItem.DisplayName;
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
                routerItem.Key = menuItem.MenuItem.Name;
            }

            bool CompareUri(MenuItemViewModel item)
            {
                var uriStr = item.MenuItem.Url;
                if (uriStr is null) return false;
                var menuUri = uriStr.StartsWith("http")
                    ? new Uri(uriStr)
                    : new Uri(baseUri, uriStr);
                return 0 == Uri.Compare(menuUri, uri, UriComponents.PathAndQuery, UriFormat.UriEscaped, StringComparison.InvariantCultureIgnoreCase);
            }
        }
        else if (routerItem.Key != "#")
        {
            menuItem = Menu.LeafItems.FirstOrDefault(u => u.MenuItem.Name == routerItem.Key);
        }

        return menuItem;
    }

    protected virtual RenderFragment GenerateMenuItem(MenuItemViewModel model)
    {
        var item = model.MenuItem;

        return builder =>
        {
            builder.OpenElement(0, "a");
            var cssClass = "menu-item";
            if (model.IsActive) cssClass += " selected";
            if (item.IsLeaf)
            {
                var url = item.Url is null ? "#" : item.Url.TrimStart('/', '~');
                builder.AddAttribute(1, "href", url);
                builder.AddAttribute(2, "target", item.Target);
                builder.AddAttribute(3, "id", item.ElementId);

                if (!item.CssClass.IsNullOrEmpty())
                    cssClass += " " + item.CssClass;
            }
            else
            {
                builder.AddAttribute(1, "onclick", () => ToggleMenuAsync(model));
                builder.AddEventPreventDefaultAttribute(2, "onclick", true);
            }

            builder.AddAttribute(4, "class", cssClass);
            builder.AddContent(5, GenerateMenuItemInner(item));
            if (!item.IsLeaf)
            {
                builder.AddContent(6, GenerateMenuItemDrop(model));
            }

            builder.CloseElement();
        };
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