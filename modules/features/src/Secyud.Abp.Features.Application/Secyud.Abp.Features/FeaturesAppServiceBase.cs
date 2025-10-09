using Secyud.Abp.Features.Localization;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Features;

public abstract class FeaturesAppServiceBase : ApplicationService
{
    protected FeaturesAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpFeaturesApplicationModule);
        LocalizationResource = typeof(AbpFeaturesResource);
    }
}
