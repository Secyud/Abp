using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Features;

public class FeatureDefinitionManager(IFeatureDefinitionStore store) : IFeatureDefinitionManager, ISingletonDependency
{
    protected IFeatureDefinitionStore Store { get; } = store;

    public virtual async Task<FeatureDefinition> GetAsync(string name)
    {
        var permission = await GetOrNullAsync(name);
        if (permission == null)
        {
            throw new AbpException("Undefined feature: " + name);
        }

        return permission;
    }

    public virtual async Task<FeatureDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return await Store.GetOrNullAsync(name);
    }

    public virtual async Task<IReadOnlyList<FeatureDefinition>> GetAllAsync()
    {
        return await Store.GetFeaturesAsync();
    }

    public virtual async Task<IReadOnlyList<FeatureGroupDefinition>> GetGroupsAsync()
    {
        return await Store.GetGroupsAsync();
    }
}