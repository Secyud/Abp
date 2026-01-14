using Microsoft.Extensions.Localization;
using Volo.Abp;

namespace Secyud.Abp.Ui.Navigation;

public class ApplicationMenu(string name) : IWithMenuItems
{
    public string Name { get; } = Check.NotNullOrWhiteSpace(name, nameof(name));
    public ApplicationMenuItemList Items { get; } = [];

    public ApplicationMenuItem AddItem(IStringLocalizer localizer, string name, string? displayName = null,
        string? icon = null,
        int order = 1000)
    {
        var item = new ApplicationMenuItem(name, localizer[$"Menu:{displayName ?? name}"], icon, order);
        Items.Add(item);
        return item;
    }

    public Dictionary<string, object> CustomData => field ??= [];

    public ApplicationMenu WithCustomData(string key, object value)
    {
        CustomData[key] = value;
        return this;
    }

    public override string ToString()
    {
        return $"[ApplicationMenu] Name = {Name}";
    }
}