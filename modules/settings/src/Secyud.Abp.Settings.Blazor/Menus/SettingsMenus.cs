using Secyud.Abp.AspNetCore.Components.Navigations;

namespace Secyud.Abp.Settings.Menus;

public class SettingsMenus
{
    public static MenuItem Default { get; } = new("Settings")
    {
        Url = DefaultUrl,
        Icon = "fa fa-cog",
        RequiredFeatures = [SettingsFeatures.Enable]
    };

    public const string DefaultUrl = "/settings";
}