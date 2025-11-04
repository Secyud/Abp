using Secyud.Abp.Tenants.Localization;
using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.Tenants.Navigation;

public class TenantsBlazorMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var administrationMenu = context.Menu.GetAdministration();

        var l = context.GetLocalizer<AbpTenantsResource>();

        administrationMenu.AddItem(
            TenantsMenuNames.Group.Create(l)
                .AddItem(TenantsMenuNames.Tenants.Create(l))
        );

        return Task.CompletedTask;
    }
}