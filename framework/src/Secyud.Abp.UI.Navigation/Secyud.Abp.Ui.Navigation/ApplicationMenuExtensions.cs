using Microsoft.Extensions.Localization;
using Volo.Abp;

namespace Secyud.Abp.Ui.Navigation;

public static class ApplicationMenuExtensions
{
    extension(ApplicationMenu menu)
    {
        public ApplicationMenuItem GetAdministration()
        {
            return menu.GetMenuItem(
                DefaultMenuNames.Application.Main.Administration
            );
        }

        public ApplicationMenuItem GetMenuItem(string menuItemName)
        {
            var menuItem = menu.GetMenuItemOrNull(menuItemName);
            if (menuItem == null)
            {
                throw new AbpException($"Could not find a menu item with given name: {menuItemName}");
            }

            return menuItem;
        }

        public ApplicationMenuItem? GetMenuItemOrNull(string menuItemName)
        {
            Check.NotNull(menu, nameof(menu));

            return menu.Items.FirstOrDefault(mi => mi.Name == menuItemName);
        }

        public bool TryRemoveMenuItem(string menuItemName)
        {
            Check.NotNull(menu, nameof(menu));

            return menu.Items.RemoveAll(item => item.Name == menuItemName) > 0;
        }
    }

    public const string CustomDataComponentKey = "ApplicationMenu.CustomComponent";

    extension(IWithMenuItems withMenuItems)
    {
        public ApplicationMenuItem AddLeaf(IStringLocalizer localizer, string name, string url,
            string? displayName = null, string? icon = null, int order = 1000)
        {
            var item = withMenuItems.AddItem(localizer, name, displayName, icon, order);
            item.IsLeaf = true;
            item.Url = url;
            return item;
        }
    }

    extension(ApplicationMenuItem menuItem)
    {
        public ApplicationMenuItem UseComponent<TComponent>()
        {
            return menuItem.UseComponent(typeof(TComponent));
        }

        public ApplicationMenuItem UseComponent(Type componentType)
        {
            return menuItem.WithCustomData(CustomDataComponentKey, componentType);
        }

        public Type? GetComponentTypeOrDefault()
        {
            if (menuItem.CustomData.TryGetValue(CustomDataComponentKey, out var value))
            {
                return value as Type;
            }

            return null;
        }
    }
}