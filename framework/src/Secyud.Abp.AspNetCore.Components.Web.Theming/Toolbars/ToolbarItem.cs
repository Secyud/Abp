using JetBrains.Annotations;
using Volo.Abp;

namespace Secyud.Abp.AspNetCore.Toolbars;

public class ToolbarItem(Type componentType, int order = 0)
{
    public Type ComponentType { get; set; } = Check.NotNull(componentType, nameof(componentType));

    public int Order { get; set; } = order;
}
