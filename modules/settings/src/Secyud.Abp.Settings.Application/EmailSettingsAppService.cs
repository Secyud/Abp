using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Emailing;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Settings;

[Authorize(SettingsPermissions.Emailing)]
public class EmailSettingsAppService(ISettingManager settingManager, IEmailSender emailSender) : SettingsAppServiceBase, IEmailSettingsAppService
{
    protected ISettingManager SettingManager { get; } = settingManager;

    protected IEmailSender EmailSender { get; } = emailSender;

    public virtual async Task<EmailSettingsDto> GetAsync()
    {
        await CheckFeatureAsync();

        var settingsDto = new EmailSettingsDto
        {
            SmtpHost = await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Host),
            SmtpPort = Convert.ToInt32(await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Port)),
            SmtpUserName = await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.UserName),
            SmtpDomain = await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Domain),
            SmtpEnableSsl = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.EnableSsl)),
            SmtpUseDefaultCredentials = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.UseDefaultCredentials)),
            DefaultFromAddress = await SettingProvider.GetOrNullAsync(EmailSettingNames.DefaultFromAddress),
            DefaultFromDisplayName = await SettingProvider.GetOrNullAsync(EmailSettingNames.DefaultFromDisplayName),
        };

        if (CurrentTenant.IsAvailable)
        {
            settingsDto.SmtpHost = await SettingManager.GetOrNullForTenantAsync(EmailSettingNames.Smtp.Host, CurrentTenant.GetId(), false);
            settingsDto.SmtpUserName = await SettingManager.GetOrNullForTenantAsync(EmailSettingNames.Smtp.UserName, CurrentTenant.GetId(), false);
            settingsDto.SmtpDomain = await SettingManager.GetOrNullForTenantAsync(EmailSettingNames.Smtp.Domain, CurrentTenant.GetId(), false);
        }

        return settingsDto;
    }

    public virtual async Task UpdateAsync(UpdateEmailSettingsDto input)
    {
        await CheckFeatureAsync();

        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.Host, input.SmtpHost);
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.Port, input.SmtpPort.ToString());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.UserName, input.SmtpUserName);
        if (!input.SmtpPassword.IsNullOrWhiteSpace())
        {
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.Password, input.SmtpPassword);
        }
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.Domain, input.SmtpDomain);
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.EnableSsl, input.SmtpEnableSsl.ToString());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.UseDefaultCredentials, input.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.DefaultFromAddress, input.DefaultFromAddress);
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.DefaultFromDisplayName, input.DefaultFromDisplayName);
    }

    [Authorize(SettingsPermissions.EmailingTest)]
    public virtual async Task SendTestEmailAsync(SendTestEmailInput input)
    {
        await CheckFeatureAsync();

        try
        {
            await EmailSender.SendAsync(input.SenderEmailAddress, input.TargetEmailAddress, input.Subject, input.Body);
        }
        catch (Exception e)
        {
            Logger.LogError("Error sending test email: " + e);
            throw new UserFriendlyException(L["MailSendingFailed"]);
        }
    }

    protected virtual async Task CheckFeatureAsync()
    {
        await FeatureChecker.CheckEnabledAsync(SettingsFeatures.Enable);
        if (CurrentTenant.IsAvailable)
        {
            await FeatureChecker.CheckEnabledAsync(SettingsFeatures.AllowChangingEmailSettings);
        }
    }
}
