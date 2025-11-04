using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Secyud.Abp.Settings.Localization;
using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.Settings.Menus;

public class SettingsMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var settingsPageOptions = context.ServiceProvider.GetRequiredService<IOptions<SettingsComponentOptions>>().Value;
        var settingPageCreationContext = new SettingComponentCreationContext(context.ServiceProvider);
        if (settingsPageOptions.Contributors.Count == 0 ||
            !await CheckAnyOfPagePermissionsGranted(settingsPageOptions, settingPageCreationContext))
        {
            return;
        }

        var l = context.GetLocalizer<AbpSettingsResource>();

        context.Menu
            .GetAdministration()
            .AddItem(
                SettingsMenus.Default.Create(l)
            );
    }

    protected virtual async Task<bool> CheckAnyOfPagePermissionsGranted(
        SettingsComponentOptions settingsComponentOptions,
        SettingComponentCreationContext settingComponentCreationContext)
    {
        foreach (var contributor in settingsComponentOptions.Contributors)
        {
            if (await contributor.CheckPermissionsAsync(settingComponentCreationContext))
            {
                return true;
            }
        }

        return false;
    }
}