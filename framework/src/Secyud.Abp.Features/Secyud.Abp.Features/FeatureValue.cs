using Volo.Abp;

namespace Secyud.Abp.Features;

[Serializable]
public class FeatureValue : NameValue
{
    public FeatureValue()
    {

    }

    public FeatureValue(string name, string value)
    {
        Name = name;
        Value = value;
    }
}
