using Secyud.Abp.Features.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Secyud.Abp.Features;

public abstract class AbpFeaturesComponentBase : AbpComponentBase
{
    protected AbpFeaturesComponentBase()
    {
        LocalizationResource = typeof(AbpFeaturesResource);
    }
}
