namespace Secyud.Abp.Features;

public interface IFeatureValueProvider
{
    string Name { get; }

    Task<string?> GetOrNullAsync(FeatureDefinition feature);
}
