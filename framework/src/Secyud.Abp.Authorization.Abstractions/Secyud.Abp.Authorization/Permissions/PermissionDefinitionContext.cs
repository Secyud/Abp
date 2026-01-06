using Volo.Abp;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionDefinitionContext(IServiceProvider serviceProvider)
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public Dictionary<string, IPermissionDefinition> Groups { get; } = [];

    public virtual void AddGroup(IPermissionDefinition group)
    {
        if (Groups.TryAdd(group.Name, group)) return;

        throw new AbpException($"There is already an existing permission group with name: {group.Name}");
    }

    public virtual IPermissionDefinition GetGroup(string name)
    {
        var group = GetGroupOrNull(name);

        return group ?? throw new AbpException($"Could not find a permission definition group with the given name: {name}");
    }

    public virtual IPermissionDefinition? GetGroupOrNull(string name)
    {
        Check.NotNull(name, nameof(name));

        return Groups.GetValueOrDefault(name);
    }

    public virtual void RemoveGroup(string name)
    {
        Check.NotNull(name, nameof(name));

        if (Groups.Remove(name)) return;
        
        throw new AbpException($"Not found permission group with name: {name}");
    }
}