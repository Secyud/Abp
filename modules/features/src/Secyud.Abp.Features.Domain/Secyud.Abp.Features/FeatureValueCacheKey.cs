using Volo.Abp.Text.Formatting;

namespace Secyud.Abp.Features;

public readonly record struct FeatureValueCacheKey(string Name, string? ProviderName, string? ProviderKey)
{
    private const string CacheKeyFormat = "pn:{0},pk:{1},n:{2}";

    public override string ToString()
    {
        return string.Format(CacheKeyFormat, ProviderName, ProviderKey, Name);
    }

    public static FeatureValueCacheKey Create(string cacheKey)
    {
        var result = FormattedStringValueExtracter.Extract(cacheKey, CacheKeyFormat, true);

        var name = result.Matches[2].Value;
        var providerName = result.Matches[0].Value;
        providerName = providerName.IsNullOrEmpty() ? null : providerName;
        var providerKey = result.Matches[1].Value;
        providerKey = providerKey.IsNullOrEmpty() ? null : providerKey;

        return new FeatureValueCacheKey(name, providerName, providerKey);
    }
}