using Secyud.Abp.Tenants.Localization;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Tenants;

public abstract class TenantsAppServiceBase : ApplicationService
{
    protected TenantsAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpTenantsApplicationModule);
        LocalizationResource = typeof(AbpTenantsResource);
    }
}
