using Secyud.Abp.Identities;
using Secyud.Abp.Identities.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Settings;

namespace Secyud.Abp.Accounts.Emailing;

public class AccountIdentityUserCreatedEventHandler(
    IdentityUserManager userManager,
    IAccountEmailer accountEmailer,
    ISettingProvider settingProvider)
    :
        IDistributedEventHandler<IdentityUserCreatedEto>,
        ITransientDependency
{
    protected IdentityUserManager UserManager { get; } = userManager;
    protected IAccountEmailer AccountEmailer { get; } = accountEmailer;
    protected ISettingProvider SettingProvider { get; } = settingProvider;

    public async Task HandleEventAsync(IdentityUserCreatedEto eventData)
    {
        if (eventData.Properties["SendConfirmationEmail"] == true.ToString().ToUpper() &&
            await SettingProvider.IsTrueAsync(IdentitiesSettingNames.SignIn.RequireConfirmedEmail))
        {
            var user = await UserManager.GetByIdAsync(eventData.Id);
            var confirmationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            await AccountEmailer.SendEmailConfirmationLinkAsync(user, confirmationToken,
                eventData.Properties.GetOrDefault("AppName") ?? "MVC");
        }
    }
}
