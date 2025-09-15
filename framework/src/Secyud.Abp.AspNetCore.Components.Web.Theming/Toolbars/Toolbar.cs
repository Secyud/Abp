using JetBrains.Annotations;
using Volo.Abp;

namespace Secyud.Abp.AspNetCore.Toolbars;

public class Toolbar
{
    public string Name { get; }

    public List<ToolbarItem> Items { get; }

    public Toolbar([NotNull] string name)
    {
        Name = Check.NotNull(name, nameof(name));
        Items = new List<ToolbarItem>();
    }
}
