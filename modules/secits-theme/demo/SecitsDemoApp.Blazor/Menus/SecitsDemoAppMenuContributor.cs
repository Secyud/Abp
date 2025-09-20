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
        
        context.Menu.AddItem(new ApplicationMenuItem(SecitsDemoAppMenus.Prefix, displayName: "Test1", "/", icon: "fa fa-globe"));
        context.Menu.AddItem(new ApplicationMenuItem("Test2", displayName: "Test2", "/", icon: "fa fa-globe")
            .AddItem(new ApplicationMenuItem("Test3", displayName: "Test3", "/", icon: "fa fa-globe"))
        );

        return Task.CompletedTask;
    }
}
