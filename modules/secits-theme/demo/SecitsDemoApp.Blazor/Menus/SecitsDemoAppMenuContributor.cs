using Volo.Abp.UI.Navigation;

namespace SecitsDemoApp.Menus;

public class SecitsDemoAppMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        context.Menu.AddItem(new ApplicationMenuItem(SecitsDemoAppMenus.Prefix, displayName: "SecitsDemoApp", "/SecitsDemoApp", icon: "fa fa-globe"));

        return Task.CompletedTask;
    }
}
