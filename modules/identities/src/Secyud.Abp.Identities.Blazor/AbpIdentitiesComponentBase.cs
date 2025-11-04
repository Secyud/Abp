using Secyud.Abp.Identities.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Secyud.Abp.Identities;

public abstract class AbpIdentitiesComponentBase : AbpComponentBase
{
    protected AbpIdentitiesComponentBase()
    {
        LocalizationResource = typeof(AbpIdentitiesResource);
        ObjectMapperContext = typeof(AbpIdentitiesBlazorModule);
    }
}