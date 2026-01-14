using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Features;

public abstract class FeatureDefinitionProvider : IFeatureDefinitionProvider, ITransientDependency
{
    public abstract void Define(FeatureDefinitionContext context);
}
