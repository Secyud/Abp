using Volo.Abp;
using Volo.Abp.ExceptionHandling;

namespace Secyud.Abp.GlobalFeatures;

[Serializable]
public class AbpGlobalFeatureNotEnabledException(
    string? message = null,
    string? code = null,
    Exception? innerException = null)
    : AbpException(message, innerException), IHasErrorCode
{
    public string? Code { get; } = code;

    public AbpGlobalFeatureNotEnabledException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
}
