using JetBrains.Annotations;

namespace Secyud.Abp.AspNetCore.Components;

public class AbpDynamicLayoutComponentOptions
{
    /// <summary>
    /// Used to define components that renders in the layout
    /// </summary>
    [NotNull]
    public Dictionary<Type, IDictionary<string,object>?> Components { get; set; } = new();
}