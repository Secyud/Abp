using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Features;

public class FeaturesTestData : ISingletonDependency
{
    public Guid User1Id { get; } = Guid.NewGuid();
}
