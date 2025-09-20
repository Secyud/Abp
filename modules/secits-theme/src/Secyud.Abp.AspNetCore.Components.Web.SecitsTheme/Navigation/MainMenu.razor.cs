using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Secyud.Secits.Blazor.Element;
using Secyud.Secits.Blazor.Icons;
using Secyud.Secits.Blazor.PageRouters;
using Volo.Abp.AspNetCore.Components.Web.Security;

namespace Secyud.Abp.AspNetCore.Navigation;

public partial class MainMenu : IDisposable
{
    [Inject]
    protected IIconProvider IconProvider { get; set; } = null!;

    [Inject]
    protected MainMenuProvider MainMenuProvider { get; set; } = null!;

    [Inject]
    protected PageRouterManager PageRouterManager { get; set; } = null!;

    [Inject]
    protected ApplicationConfigurationChangedService ApplicationConfigurationChangedService { get; set; } = null!;

    protected MenuViewModel? Menu { get; set; }

    private void MenuStateChanged(object? sender, EventArgs e)
    {
        if (Menu is null) return;

        var menuItem = FindMenuItemByRouterItem(
            PageRouterManager.CurrentItem);

        if (menuItem is null)
        {
            Menu.DeactivateAll();
        }
        else
        {
            Menu.Activate(menuItem);
        }

        InvokeAsync(StateHasChanged);
    }

    protected override async Task OnInitializedAsync()
    {
        Menu = await MainMenuProvider.GetMenuAsync();
        ApplicationConfigurationChangedService.Changed +=
            ApplicationConfigurationChanged;
        PageRouterManager.StateChanged += MenuStateChanged;
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

    protected virtual MenuItemViewModel? FindMenuItemByRouterItem(PageRouterItem? routerItem)
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
                return menuUri.PathAndQuery == uri.PathAndQuery;
            }
        }
        else if (routerItem.Key != "#")
        {
            menuItem = Menu.LeafItems.FirstOrDefault(
                u => u.MenuItem.Name == routerItem.Key);
        }

        return menuItem;
    }

    public void Dispose()
    {
        PageRouterManager.StateChanged -= MenuStateChanged;
        ApplicationConfigurationChangedService.Changed -= ApplicationConfigurationChanged;
    }

    protected virtual RenderFragment GenerateMenuItem(MenuItemViewModel model)
    {
        var item = model.MenuItem;

        return builder =>
        {
            builder.OpenElement(0, "a");
            var cssClass = "secits-menu-item";
            if (item.IsLeaf)
            {
                var url = item.Url == null ? "#" : item.Url.TrimStart('/', '~');
                builder.AddAttribute(1, "href", url);
                builder.AddAttribute(2, "target", item.Target);
                builder.AddAttribute(3, "id", item.ElementId);

                if (!item.CssClass.IsNullOrEmpty())
                    cssClass += " " + item.CssClass;
                if (model.IsActive)
                    cssClass += " selected";
            }
            else
            {
                builder.AddAttribute(1, "onclick", () => ToggleMenuAsync(model));
                builder.AddEventPreventDefaultAttribute(2, "onclick", true);
                if (model.IsActive || model.IsOpen)
                    cssClass += " selected";
            }

            builder.AddAttribute(4, "class", cssClass);
            builder.AddContent(5, GenerateMenuItemInner(item));
            builder.OpenComponent<SIcon>(6);
            builder.AddComponentParameter(7, "Class", "secits-menu-item-edge");
            builder.AddComponentParameter(8, "IconName", IconProvider.GetIcon(model.IsOpen ? IconName.Asc : IconName.Desc));
            builder.CloseElement();
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
}