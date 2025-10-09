using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Settings;

[Serializable]
[IgnoreMultiTenancy]
public class SettingCacheItem
{
    public string? Value { get; set; }

    public SettingCacheItem()
    {
    }

    public SettingCacheItem(string? value)
    {
        Value = value;
    }
}