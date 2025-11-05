using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Secyud.Abp.Account.Localization;
using Secyud.Abp.Accounts.Emailing.Templates;
using Secyud.Abp.Identities;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;
using Volo.Abp.UI.Navigation.Urls;

namespace Secyud.Abp.Accounts.Emailing;

public class AccountEmailer(
    IEmailSender emailSender,
    ITemplateRenderer templateRenderer,
    IStringLocalizer<AbpAccountsResource> stringLocalizer,
    IAppUrlProvider appUrlProvider,
    ICurrentTenant currentTenant)
    : IAccountEmailer, ITransientDependency
{
    public ILogger<AccountEmailer> Logger { get; set; } = NullLogger<AccountEmailer>.Instance;

    protected ITemplateRenderer TemplateRenderer { get; } = templateRenderer;
    protected IEmailSender EmailSender { get; } = emailSender;
    protected IStringLocalizer<AbpAccountsResource> StringLocalizer { get; } = stringLocalizer;
    protected IAppUrlProvider AppUrlProvider { get; } = appUrlProvider;
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;

    public virtual async Task SendPasswordResetLinkAsync(
        IdentityUser user,
        string resetToken,
        string appName,
        string? returnUrl = null,
        string? returnUrlHash = null)
    {
        Debug.Assert(CurrentTenant.Id == user.TenantId, "This method can only work for current tenant!");

        var url = await AppUrlProvider.GetResetPasswordUrlAsync(appName);

        //TODO: Use AbpAspNetCoreMultiTenancyOptions to get the key
        var link = $"{url}?userId={user.Id}&{TenantResolverConsts.DefaultTenantKey}={user.TenantId}&resetToken={UrlEncoder.Default.Encode(resetToken)}";

        if (!returnUrl.IsNullOrEmpty())
        {
            link += "&returnUrl=" + NormalizeReturnUrl(returnUrl);
        }

        if (!returnUrlHash.IsNullOrEmpty())
        {
            link += "&returnUrlHash=" + returnUrlHash;
        }

        var emailContent = await TemplateRenderer.RenderAsync(
            AccountEmailTemplates.PasswordResetLink,
            new { link }
        );

        try
        {
            await EmailSender.QueueAsync(
                user.Email,
                StringLocalizer["PasswordReset"],
                emailContent
            );
        }
        catch (Exception e)
        {
            Logger.LogException(e);
            throw new UserFriendlyException(StringLocalizer["MailSendingFailed"]);
        }
    }

    public virtual async Task SendEmailConfirmationLinkAsync(
        IdentityUser user,
        string confirmationToken,
        string appName,
        string? returnUrl = null,
        string? returnUrlHash = null)
    {
        Debug.Assert(CurrentTenant.Id == user.TenantId, "This method can only work for current tenant!");

        var url = await AppUrlProvider.GetEmailConfirmationUrlAsync(appName);

        //TODO: Use AbpAspNetCoreMultiTenancyOptions to get the key
        var link =
            $"{url}?userId={user.Id}&{TenantResolverConsts.DefaultTenantKey}={user.TenantId}&confirmationToken={UrlEncoder.Default.Encode(confirmationToken)}";

        if (!returnUrl.IsNullOrEmpty())
        {
            link += "&returnUrl=" + NormalizeReturnUrl(returnUrl);
        }

        if (!returnUrlHash.IsNullOrEmpty())
        {
            link += "&returnUrlHash=" + returnUrlHash;
        }

        var emailContent = await TemplateRenderer.RenderAsync(
            AccountEmailTemplates.EmailConfirmationLink,
            new { link }
        );

        try
        {
            await EmailSender.QueueAsync(
                user.Email,
                StringLocalizer["EmailConfirmation"],
                emailContent
            );
        }
        catch (Exception e)
        {
            Logger.LogException(e);
            throw new UserFriendlyException(StringLocalizer["MailSendingFailed"]);
        }
    }

    public virtual async Task SendEmailSecurityCodeAsync(IdentityUser user, string code)
    {
        var emailContent = await TemplateRenderer.RenderAsync(
            AccountEmailTemplates.EmailSecurityCode,
            new { code }
        );

        try
        {
            await EmailSender.QueueAsync(
                user.Email,
                StringLocalizer["EmailSecurityCodeSubject"],
                emailContent
            );
        }
        catch (Exception e)
        {
            Logger.LogException(e);
            throw new UserFriendlyException(StringLocalizer["MailSendingFailed"]);
        }
    }

    private string NormalizeReturnUrl(string returnUrl)
    {
        if (returnUrl.IsNullOrEmpty())
        {
            return returnUrl;
        }

        //Handling openid connect login
        if (returnUrl.StartsWith("/connect/authorize/callback", StringComparison.OrdinalIgnoreCase))
        {
            if (returnUrl.Contains("?"))
            {
                var queryPart = returnUrl.Split('?')[1];
                var queryParameters = queryPart.Split('&');
                foreach (var queryParameter in queryParameters)
                {
                    if (queryParameter.Contains("="))
                    {
                        var queryParam = queryParameter.Split('=');
                        if (queryParam[0] == "redirect_uri")
                        {
                            return HttpUtility.UrlDecode(queryParam[1]);
                        }
                    }
                }
            }
        }

        if (returnUrl.StartsWith("/connect/authorize?", StringComparison.OrdinalIgnoreCase))
        {
            return HttpUtility.UrlEncode(returnUrl);
        }

        return returnUrl;
    }
}