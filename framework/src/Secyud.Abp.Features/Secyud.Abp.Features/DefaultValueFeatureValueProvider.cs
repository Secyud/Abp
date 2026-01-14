using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Features;

public class DefaultValueFeatureValueProvider : IFeatureValueProvider, ITransientDependency
{
    public const string ProviderName = "D";

    public string Name => ProviderName;

    public Task<string?> GetOrNullAsync(FeatureDefinition setting)
    {
        return Task.FromResult(setting.DefaultValue);
    }
}