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
        context.Menu.AddGroup(new ApplicationMenuGroup("Group", "Group"));
        
        context.Menu.AddItem(new ApplicationMenuItem(SecitsDemoAppMenus.Prefix, displayName: "Test1", "/", icon: "fa fa-globe"));
        context.Menu.AddItem(new ApplicationMenuItem("Test4", displayName: "Test1", "/", icon: "fa fa-globe", groupName: "Group"));
        context.Menu.AddItem(new ApplicationMenuItem("Test2", displayName: "Test2", "/", icon: "fa fa-globe")
            .AddItem(new ApplicationMenuItem("Test3", displayName: "Test3", "/test-page1", icon: "fa fa-globe"))
        );

        return Task.CompletedTask;
    }
}
