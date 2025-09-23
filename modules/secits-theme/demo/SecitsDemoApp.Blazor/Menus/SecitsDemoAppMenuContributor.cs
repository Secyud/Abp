using SecitsDemoApp.Localization;
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
        var l = context.GetLocalizer(typeof(SecitsDemoAppResource));

        context.Menu
            // index
            .AddItem(new ApplicationMenuItem(AppMenus.Index, l[AppMenus.Index], AppMenus.IndexUri))
            // grid
            .AddItem(new ApplicationMenuItem(AppMenus.Grid.Name, l[AppMenus.Grid.Name])
                .AddItem(new ApplicationMenuItem(AppMenus.Grid.Paged, l[AppMenus.Grid.Paged], AppMenus.Grid.PagedUri))
                .AddItem(new ApplicationMenuItem(AppMenus.Grid.Visualized, l[AppMenus.Grid.Visualized], AppMenus.Grid.VisualizedUri))
            )
            // input
            .AddItem(new ApplicationMenuItem(AppMenus.Theme.Name, l[AppMenus.Theme.Name])
                .AddItem(new ApplicationMenuItem(AppMenus.Theme.Button, l[AppMenus.Theme.Button], AppMenus.Theme.ButtonUri))
                .AddItem(new ApplicationMenuItem(AppMenus.Theme.Input, l[AppMenus.Theme.Input], AppMenus.Theme.InputUri))
            )
            ;

        return Task.CompletedTask;
    }
}