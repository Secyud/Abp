using SecitsDemoApp.Localization;
using Volo.Abp.Application.Services;

namespace SecitsDemoApp;

public abstract class SecitsDemoAppAppService : ApplicationService
{
    protected SecitsDemoAppAppService()
    {
        LocalizationResource = typeof(SecitsDemoAppResource);
        ObjectMapperContext = typeof(SecitsDemoAppApplicationModule);
    }
}
