using SecitsDemoApp.Localization;
using Volo.Abp.AspNetCore.Components;

namespace SecitsDemoApp;

public abstract class SecitsDemoAppComponentBase : AbpComponentBase
{
    protected SecitsDemoAppComponentBase()
    {
        LocalizationResource = typeof(SecitsDemoAppResource);
    }
}