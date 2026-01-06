using Secyud.Abp.Ui.Navigation;

namespace Secyud.Abp.Secits.AspNetCore.Components.Navigation;

public class MenuViewModel(ApplicationMenu menu)
{
    public List<MenuItemViewModel> Items { get; } = [];
    public List<MenuItemViewModel> LeafItems { get; } = [];

    public string Name { get; set; } = menu.Name;

    public void AddMenuItem(MenuItemViewModel item)
    {
        item.Parent = null;
        Items.Add(item);
    }

    public void RemoveMenuItem(MenuItemViewModel item)
    {
        Items.Remove(item);
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