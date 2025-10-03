using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Features;

[Serializable]
[IgnoreMultiTenancy]
public class FeatureValueCacheItem
{
    public string? Value { get; set; }

    public FeatureValueCacheItem()
    {
    }

    public FeatureValueCacheItem(string? value)
    {
        Value = value;
    }
}