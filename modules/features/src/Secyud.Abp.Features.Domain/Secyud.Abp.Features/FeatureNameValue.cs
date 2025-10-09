using Volo.Abp;

namespace Secyud.Abp.Features;

[Serializable]
public class FeatureNameValue : NameValue<string?>
{
    public FeatureNameValue()
    {

    }

    public FeatureNameValue(string name, string? value)
    {
        Name = name;
        Value = value;
    }
}
