namespace Secyud.Abp.Features;

public interface IFeatureStore
{
    Task<string?> GetOrNullAsync(string name, string? providerName, string? providerKey);
}
