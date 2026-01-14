using Volo.Abp;
using Volo.Abp.Localization;

namespace Secyud.Abp.Features;

public class FeatureDefinitionContext
{
    public Dictionary<string, FeatureGroupDefinition> Groups { get; } = new();

    public virtual void AddGroup(FeatureGroupDefinition group)
    {
        if (Groups.TryAdd(group.Name, group)) return;

        throw new AbpException($"There is already an existing feature group with name: {group.Name}");
    }

    public virtual FeatureGroupDefinition GetGroup(string name)
    {
        var group = GetGroupOrNull(name);

        return group ??
               throw new AbpException($"Could not find a feature definition group with the given name: {name}");
    }
    
    public FeatureGroupDefinition? GetGroupOrNull(string name)
    {
        Check.NotNull(name, nameof(name));

        return Groups.GetValueOrDefault(name);
    }

    public void RemoveGroup(string name)
    {
        Check.NotNull(name, nameof(name));

        if (Groups.Remove(name)) return;
        
        throw new AbpException($"Undefined feature group: '{name}'.");
    }
}