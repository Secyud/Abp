using Microsoft.Extensions.Localization;
using Secyud.Abp.Account.Localization;
using Secyud.Abp.Identities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Sms;

namespace Secyud.Abp.Accounts.Phone;

public class AccountPhoneService(ISmsSender smsSender, IStringLocalizer<AbpAccountsResource> localizer) : IAccountPhoneService, ITransientDependency
{
    protected IStringLocalizer<AbpAccountsResource> Localizer { get; } = localizer;
    protected ISmsSender SmsSender { get; } = smsSender;

    public virtual async Task SendConfirmationCodeAsync(IdentityUser user, string confirmationToken)
    {
        var name = string.IsNullOrWhiteSpace(user.Name)
            ? user.UserName
            : $"{user.Name}{user.Surname?.EnsureStartsWith(' ')}";

        if (user.PhoneNumber is null)
            throw new ArgumentNullException(nameof(user.PhoneNumber));

        await SmsSender.SendAsync(user.PhoneNumber, Localizer["PhoneConfirmationSms", name, confirmationToken]);
    }

    public virtual async Task SendSecurityCodeAsync(IdentityUser user, string code)
    {
        if (user.PhoneNumber is null)
            throw new ArgumentNullException(nameof(user.PhoneNumber));

        await SmsSender.SendAsync(user.PhoneNumber, Localizer["EmailSecurityCodeBody", code]);
    }
}