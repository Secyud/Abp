namespace Secyud.Abp.Secits.AspNetCore.Components.Toolbars;

public class ToolbarItem(Type componentType, int order = 0, bool fix = false)
{
    public Type ComponentType { get; } = componentType;
    public int Order { get; } = order;
    public bool Fix { get; } = fix;
}