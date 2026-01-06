using Localization.Resources.AbpUi;
using Secyud.Abp.Ui.Navigation.Localization.Resource;

namespace Secyud.Abp.Ui.Navigation;

public class DefaultMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        Configure(context);
        return Task.CompletedTask;
    }

    protected virtual void Configure(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<AbpUiNavigationResource>();

        if (context.Menu.Name == StandardMenus.Main)
        {
            context.Menu.AddItem(l,
                DefaultMenuNames.Application.Main.Administration,
                "Administration", icon: "fa fa-wrench"
            );
        }
    }
}