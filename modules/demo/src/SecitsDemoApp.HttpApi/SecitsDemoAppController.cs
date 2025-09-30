using SecitsDemoApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SecitsDemoApp;

public abstract class SecitsDemoAppController : AbpControllerBase
{
    protected SecitsDemoAppController()
    {
        LocalizationResource = typeof(SecitsDemoAppResource);
    }
}
