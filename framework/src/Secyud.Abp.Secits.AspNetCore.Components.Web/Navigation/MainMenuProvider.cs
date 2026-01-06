using Secyud.Abp.Ui.Navigation;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Secits.AspNetCore.Components.Navigation;

public class MainMenuProvider(IMenuManager menuManager)
    : IScopedDependency
{
    public virtual async Task<MenuViewModel> GetMenuAsync()
    {
        var menu = await menuManager.GetMainMenuAsync();

        var result = new MenuViewModel(menu);

        foreach (var item in menu.Items)
        {
            result.AddMenuItem(CreateMenuItemViewModel(item, result));
        }

        return result;
    }

    protected static MenuItemViewModel CreateMenuItemViewModel(
        ApplicationMenuItem applicationMenuItem, MenuViewModel model)
    {
        var viewModel = new MenuItemViewModel(applicationMenuItem, model);
        if (applicationMenuItem.IsLeaf)
            model.LeafItems.Add(viewModel);

        foreach (var item in applicationMenuItem.Items)
        {
            viewModel.AddChild(CreateMenuItemViewModel(item, model));
        }

        return viewModel;
    }
}