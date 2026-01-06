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

        public ApplicationMenuItem AddItem(IStringLocalizer localizer, string name,
            string? displayName = null, string? icon = null, int order = 1000)
        {
            var item = new ApplicationMenuItem(name,
                localizer[$"Menu:{displayName ?? name}"], icon, order);
            menu.Items.Add(item);
            return item;
        }

        public ApplicationMenuLeafItem AddLeaf(IStringLocalizer localizer, string name, string url,
            string? displayName = null, string? icon = null, int order = 1000)
        {
            var item = new ApplicationMenuLeafItem(name,
                localizer[$"Menu:{displayName ?? name}"], url, icon, order);
            menu.Items.Add(item);
            return item;
        }
    }

    public const string CustomDataComponentKey = "ApplicationMenu.CustomComponent";

    extension(ApplicationMenuItem menuItem)
    {
        public ApplicationMenuItem AddItem(IStringLocalizer localizer, string name,
            string? displayName = null, string? icon = null, int order = 1000)
        {
            var item = new ApplicationMenuItem(
                $"{menuItem.Name}.{name}",
                localizer[$"Menu:{displayName ?? name}"],
                icon, order, menuItem);
            menuItem.Items.Add(item);
            return item;
        }

        public ApplicationMenuLeafItem AddLeaf(IStringLocalizer localizer, string name, string url,
            string? displayName = null, string? icon = null, int order = 1000)
        {
            var item = new ApplicationMenuLeafItem(
                $"{menuItem.Name}.{name}",
                localizer[$"Menu:{displayName ?? name}"],
                url, icon, order, menuItem);
            menuItem.Items.Add(item);
            return item;
        }

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