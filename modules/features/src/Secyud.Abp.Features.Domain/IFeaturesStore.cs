namespace Secyud.Abp.Features;

public interface IFeaturesStore
{
    Task<string?> GetOrNullAsync(string name, string? providerName, string? providerKey);

    Task SetAsync(string name, string? value, string? providerName, string? providerKey);

    Task DeleteAsync(string name, string? providerName, string? providerKey);
}
