namespace Secyud.Abp.Features;

public interface IFeatureChecker
{
    Task<string?> GetOrNullAsync(string name);

    Task<bool> IsEnabledAsync(string name);
}
