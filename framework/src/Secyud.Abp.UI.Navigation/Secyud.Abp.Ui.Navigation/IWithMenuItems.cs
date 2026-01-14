using Microsoft.Extensions.Localization;

namespace Secyud.Abp.Ui.Navigation;

public interface IWithMenuItems
{
    /// <summary>
    /// Menu items.
    /// </summary>
    ApplicationMenuItemList? Items { get; }

    ApplicationMenuItem AddItem(IStringLocalizer localizer, string name,
        string? displayName = null, string? icon = null, int order = 1000);
}