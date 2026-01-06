using Volo.Abp;

namespace Secyud.Abp.Ui.Navigation;

public class ApplicationMenu(string name) : IHasMenuItems
{
    public string Name { get; } = Check.NotNullOrWhiteSpace(name, nameof(name));
    public ApplicationMenuItemList Items { get; } = [];

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