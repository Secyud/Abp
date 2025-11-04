using Secyud.Abp.Identities.Localization;
using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.Identities.Navigation;

public class AbpIdentitiesBlazorMainMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }


        var administrationMenu = context.Menu.GetAdministration();

        var l = context.GetLocalizer<AbpIdentitiesResource>();

        administrationMenu.AddItem(
            IdentitiesMenus.Group.Create(l)
                .AddItem(IdentitiesMenus.Roles.Create(l))
                .AddItem(IdentitiesMenus.Users.Create(l))
                .AddItem(IdentitiesMenus.ClaimTypes.Create(l))
                .AddItem(IdentitiesMenus.SecurityLogs.Create(l))
        );

        return Task.CompletedTask;
    }
}