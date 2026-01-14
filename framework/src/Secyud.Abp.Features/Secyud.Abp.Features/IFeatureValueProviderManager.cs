namespace Secyud.Abp.Features;

public interface IFeatureValueProviderManager
{
    IReadOnlyList<IFeatureValueProvider> ValueProviders { get; }
}