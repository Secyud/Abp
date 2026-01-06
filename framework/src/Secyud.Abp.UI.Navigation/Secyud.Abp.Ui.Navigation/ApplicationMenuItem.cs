using Volo.Abp.SimpleStateChecking;

namespace Secyud.Abp.Ui.Navigation;

public class ApplicationMenuItem(
    string name,
    string displayName,
    string? icon = null,
    int order = 1000,
    ApplicationMenuItem? parent = null) :
    IHasSimpleStateCheckers<ApplicationMenuItem>, IHasMenuItems
{
    public virtual bool IsLeaf => false;
    public virtual string? Url => null;
    public string Name { get; } = name;
    public string DisplayName { get; set; } = displayName;
    public string? Icon { get; set; } = icon;
    public int Order { get; set; } = order;
    public ApplicationMenuItem? Parent { get; } = parent;
    public ApplicationMenuItemList Items => field ??= [];
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