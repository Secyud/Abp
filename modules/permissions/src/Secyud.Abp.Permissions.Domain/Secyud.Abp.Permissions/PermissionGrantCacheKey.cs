using Volo.Abp.Text.Formatting;

namespace Secyud.Abp.Permissions;

public readonly record struct PermissionGrantCacheKey(string Name, string ProviderName, string ProviderKey)
{
    private const string CacheKeyFormat = "pn:{0},pk:{1},n:{2}";

    public override string ToString()
    {
        return string.Format(CacheKeyFormat, ProviderName, ProviderKey, Name);
    }

    public static PermissionGrantCacheKey Create(string cacheKey)
    {
        var result = FormattedStringValueExtracter.Extract(cacheKey, CacheKeyFormat, true);

        return new PermissionGrantCacheKey(
            result.Matches[2].Value,
            result.Matches[0].Value,
            result.Matches[1].Value);
    }
}