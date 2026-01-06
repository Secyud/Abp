namespace Secyud.Abp.Secits.AspNetCore.Components;

public class AbpDynamicLayoutComponentOptions
{
    /// <summary>
    /// Used to define components that renders in the layout
    /// </summary>
    public Dictionary<Type, IDictionary<string,object>?> Components { get; set; } = new();
}