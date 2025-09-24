using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.AspNetCore.Navigation;

public class MenuViewModel(ApplicationMenu menu)
{
    private readonly List<MenuItemViewModel> _items = [];
    public ApplicationMenu Menu { get; } = menu;
    public IReadOnlyList<MenuItemViewModel> Items => _items;
    public List<MenuItemViewModel> LeafItems { get; } = [];

    public void AddMenuItem(MenuItemViewModel item)
    {
        item.Parent = null;
        _items.Add(item);
    }

    public void RemoveMenuItem(MenuItemViewModel item)
    {
        _items.Remove(item);
    }

    public void ToggleOpen(MenuItemViewModel menuItem)
    {
        if (menuItem.IsOpen)
        {
            menuItem.Close();
        }
        else
        {
            //CloseAll();
            menuItem.Open();
        }
    }

    public void Activate(MenuItemViewModel menuItem)
    {
        if (menuItem.IsActive)
        {
            return;
        }

        DeactivateAll();
        menuItem.Open();
        menuItem.Activate();
    }

    public void CloseAll()
    {
        foreach (var item in Items)
        {
            item.Close();
        }
    }

    public void DeactivateAll()
    {
        foreach (var item in Items)
        {
            item.Deactivate();
        }
    }
}