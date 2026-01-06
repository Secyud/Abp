using Secyud.Abp.Ui.Navigation;

namespace Secyud.Abp.Secits.AspNetCore.Components.Navigation;

public class MenuItemViewModel(ApplicationMenuItem menuItem, MenuViewModel menu)
{
    public MenuViewModel Menu { get; } = menu;
    public bool IsActive { get; set; }

    public bool IsOpen { get; set; }

    public bool IsLeaf { get; set; } = menuItem.IsLeaf;
    public string? ElementId { get; set; } = menuItem.ElementId;
    public string? Url { get; set; } = menuItem.Url;
    public string? CssClass { get; set; } = menuItem.CssClass;
    public string? Target { get; set; } = menuItem.Target;

    public Type? ComponentType { get; set; } = menuItem.GetComponentTypeOrDefault();
    public string? Icon { get; set; } = menuItem.Icon;
    public string DisplayName { get; set; } = menuItem.DisplayName;
    public string Name { get; set; } = menuItem.Name;
    public List<MenuItemViewModel> Children { get; } = [];

    public MenuItemViewModel? Parent
    {
        get;
        set
        {
            field?.Children.Remove(this);
            field = value;
            field?.Children.Add(this);
        }
    }

    public void AddChild(MenuItemViewModel item)
    {
        item.Parent = this;
    }

    public void Activate()
    {
        Parent?.Activate();
        IsActive = true;
    }

    public void Deactivate()
    {
        foreach (var childItem in Children)
        {
            childItem.Deactivate();
        }

        IsActive = false;
    }

    public void Open()
    {
        Parent?.Open();
        IsOpen = true;
    }

    public void Close()
    {
        foreach (var childItem in Children)
        {
            childItem.Close();
        }

        IsOpen = false;
    }
}