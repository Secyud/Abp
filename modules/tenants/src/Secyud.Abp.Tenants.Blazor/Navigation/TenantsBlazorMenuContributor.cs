using Secyud.Abp.Tenants.Localization;
using Volo.Abp.Authorization.Permissions;
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

        var tenantsMenuItem = new ApplicationMenuItem(
            TenantsMenuNames.GroupName,
            l["Menu:Tenants"],
            icon: "fa fa-users"
        );
        administrationMenu.AddItem(tenantsMenuItem);

        tenantsMenuItem.AddItem(new ApplicationMenuItem(
            TenantsMenuNames.Tenants,
            l["Tenants"],
            url: TenantsMenuNames.TenantsUri
        ).RequirePermissions(TenantsPermissions.Tenants.Default));

        return Task.CompletedTask;
    }
}