using Secyud.Abp.Settings.Localization;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Settings;

public abstract class SettingsAppServiceBase : ApplicationService
{
    protected SettingsAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpSettingsApplicationModule);
        LocalizationResource = typeof(AbpSettingsResource);
    }
}
