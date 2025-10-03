using JetBrains.Annotations;
using Volo.Abp;

namespace Secyud.Abp.Features;

[Serializable]
public class FeatureNameValueWithGrantedProvider : NameValue<string?>
{
    public FeatureValueProviderInfo? Provider { get; set; }

    public FeatureNameValueWithGrantedProvider(string name, string? value)
    {
        Check.NotNull(name, nameof(name));

        Name = name;
        Value = value;
    }
}