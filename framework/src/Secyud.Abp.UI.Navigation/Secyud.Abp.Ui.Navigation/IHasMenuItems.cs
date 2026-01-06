namespace Secyud.Abp.Ui.Navigation;

public interface IHasMenuItems
{
    /// <summary>
    /// Menu items.
    /// </summary>
    ApplicationMenuItemList? Items { get; }
}