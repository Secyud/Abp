using Volo.Abp;

namespace Secyud.Abp.Features;

[Serializable]
public class FeatureValueInvalidException : BusinessException
{
    public FeatureValueInvalidException(string name) :
        base(FeaturesDomainErrorCodes.FeatureValueInvalid)
    {
        WithData("0", name);
    }
}
