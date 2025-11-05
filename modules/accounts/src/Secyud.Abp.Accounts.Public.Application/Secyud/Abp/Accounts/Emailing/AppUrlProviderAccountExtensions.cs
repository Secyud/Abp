using Volo.Abp.UI.Navigation.Urls;

namespace Secyud.Abp.Accounts.Emailing;

public static class AppUrlProviderAccountExtensions
{
    public static Task<string> GetResetPasswordUrlAsync(this IAppUrlProvider appUrlProvider, string appName)
    {
        return appUrlProvider.GetUrlAsync(appName, AccountUrlNames.PasswordReset);
    }

    public static Task<string> GetEmailConfirmationUrlAsync(this IAppUrlProvider appUrlProvider, string appName)
    {
        return appUrlProvider.GetUrlAsync(appName, AccountUrlNames.EmailConfirmation);
    }
}
