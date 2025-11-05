using Secyud.Abp.Account.Localization;
using Volo.Abp.UI.Navigation;

namespace Secyud.Abp.Accounts;

public class AbpAccountBlazorUserMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.User)
        {
            return Task.CompletedTask;
        }

        var accountResource = context.GetLocalizer<AbpAccountsResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
            AbpAccountUserMenu.Manage.Name,
            accountResource["MyAccount"],
            url: AbpAccountUserMenu.Manage.Url, icon: "fa fa-cog"));

        return Task.CompletedTask;
    }
}