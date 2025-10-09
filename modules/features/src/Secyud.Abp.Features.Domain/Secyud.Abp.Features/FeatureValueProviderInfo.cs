using Volo.Abp;

namespace Secyud.Abp.Features;

[Serializable]
public class FeatureValueProviderInfo
{
    public string Name { get; }

    public string? Key { get; }

    public FeatureValueProviderInfo(string name, string? key)
    {
        Check.NotNull(name, nameof(name));

        Name = name;
        Key = key;
    }
}
