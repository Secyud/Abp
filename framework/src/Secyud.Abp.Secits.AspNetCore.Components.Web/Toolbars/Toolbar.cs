using Volo.Abp;

namespace Secyud.Abp.Secits.AspNetCore.Components.Toolbars;

public class Toolbar(string name)
{
    public string Name { get; } = Check.NotNull(name, nameof(name));

    public List<ToolbarItem> Items { get; } = new();
}
