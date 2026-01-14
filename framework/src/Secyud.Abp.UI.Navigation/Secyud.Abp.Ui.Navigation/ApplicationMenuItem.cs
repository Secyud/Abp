using Microsoft.Extensions.Localization;
using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Ui.Navigation;

public class ApplicationMenuItem(
    string name,
    string displayName,
    string? icon = null,
    int order = 1000,
    ApplicationMenuItem? parent = null) :
    IHasSimpleStateCheckers<ApplicationMenuItem>, IWithMenuItems
{
    public bool IsLeaf { get; set; }
    public string? Url { get; set; }
    public string Name { get; } = name;
    public string DisplayName { get; set; } = displayName;
    public string? Icon { get; set; } = icon;
    public int Order { get; set; } = order;
    public ApplicationMenuItem? Parent { get; } = parent;
    public ApplicationMenuItemList Items => field ??= [];

    public ApplicationMenuItem AddItem(IStringLocalizer localizer, string name,
        string? displayName = null, string? icon = null,
        int order = 1000)
    {
        var item = new ApplicationMenuItem(name, 
            localizer[$"Menu:{displayName ?? name}"],
            icon, order, this);
        Items.Add(item);
        return item;
    }

    public Dictionary<string, object> CustomData => field ??= [];
    public List<ISimpleStateChecker<ApplicationMenuItem>> StateCheckers { get; } = [];

    public string? ElementId
    {
        get;
        set => field = NormalizeElementId(value);
    } = "menuitem_" + name;

    public string? CssClass { get; set; }
    public string? Target { get; set; }

    public ApplicationMenuItem WithCustomData(string key, object value)
    {
        CustomData[key] = value;
        return this;
    }

    protected string? NormalizeElementId(string? elementId)
    {
        return elementId?.Replace('.', '_');
    }
}