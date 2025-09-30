using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Secyud.Abp.Settings.Localization;
using Secyud.Secits.Blazor.Element;
using Secyud.Secits.Blazor.Validations;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization.Permissions;

namespace Secyud.Abp.Settings.Components.EmailSettingGroup;

public partial class EmailSettingGroupViewComponent
{
    [Inject]
    protected IEmailSettingsAppService EmailSettingsAppService { get; set; } = null!;

    [Inject]
    protected IPermissionChecker PermissionChecker { get; set; } = null!;

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; } = null!;

    [Inject]
    protected IUiMessageService UiMessageService { get; set; } = null!;

    protected UpdateEmailSettingsViewModel? EmailSettings;

    protected SendTestEmailViewModel? SendTestEmailInput;

    protected ValidationForm? EmailSettingValidation;
    protected SModal? SendTestEmailModal;

    protected bool HasSendTestEmailPermission { get; set; }

    public EmailSettingGroupViewComponent()
    {
        ObjectMapperContext = typeof(AbpSettingsBlazorModule);
        LocalizationResource = typeof(AbpSettingsResource);
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            EmailSettings = ObjectMapper.Map<EmailSettingsDto, UpdateEmailSettingsViewModel>(await EmailSettingsAppService.GetAsync());
            HasSendTestEmailPermission = await PermissionChecker.IsGrantedAsync(SettingsPermissions.EmailingTest);
            SendTestEmailInput = new SendTestEmailViewModel();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task UpdateSettingsAsync()
    {
        try
        {
            if (!EmailSettingValidation!.Validate())
            {
                return;
            }

            await EmailSettingsAppService.UpdateAsync(ObjectMapper.Map<UpdateEmailSettingsViewModel, UpdateEmailSettingsDto>(EmailSettings!));

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenSendTestEmailModalAsync()
    {
        try
        {
            var emailSettings = await EmailSettingsAppService.GetAsync();
            SendTestEmailInput = new SendTestEmailViewModel
            {
                SenderEmailAddress = emailSettings.DefaultFromAddress!,
                TargetEmailAddress = CurrentUser.Email!,
                Subject = L["TestEmailSubject", new Random().Next(1000, 9999)],
                Body = L["TestEmailBody"]
            };

            await SendTestEmailModal!.ShowAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task CloseSendTestEmailModalAsync()
    {
        return SendTestEmailModal!.HideAsync();
    }

    protected virtual async Task SendTestEmailAsync()
    {
        try
        {
            if (!SendTestEmailModal!.Validate())
            {
                return;
            }

            await EmailSettingsAppService.SendTestEmailAsync(ObjectMapper.Map<SendTestEmailViewModel, SendTestEmailInput>(SendTestEmailInput!));

            await Notify.Success(L["SuccessfullySent"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    public class UpdateEmailSettingsViewModel
    {
        [MaxLength(256)]
        [Display(Name = "SmtpHost")]
        public string? SmtpHost { get; set; }

        [Range(1, 65535)]
        [Display(Name = "SmtpPort")]
        public int SmtpPort { get; set; }

        [MaxLength(1024)]
        [Display(Name = "SmtpUserName")]
        public string? SmtpUserName { get; set; }

        [MaxLength(1024)]
        [DataType(DataType.Password)]
        [DisableAuditing]
        [Display(Name = "SmtpPassword")]
        public string? SmtpPassword { get; set; }

        [MaxLength(1024)]
        [Display(Name = "SmtpDomain")]
        public string? SmtpDomain { get; set; }

        [Display(Name = "SmtpEnableSsl")]
        public bool SmtpEnableSsl { get; set; }

        [Display(Name = "SmtpUseDefaultCredentials")]
        public bool SmtpUseDefaultCredentials { get; set; }

        [MaxLength(1024)]
        [Required]
        [Display(Name = "DefaultFromAddress")]
        public string? DefaultFromAddress { get; set; }

        [MaxLength(1024)]
        [Required]
        [Display(Name = "DefaultFromDisplayName")]
        public string? DefaultFromDisplayName { get; set; }
    }

    public class SendTestEmailViewModel
    {
        [Required]
        public string SenderEmailAddress { get; set; } = string.Empty;

        [Required]
        public string TargetEmailAddress { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        public string? Body { get; set; }
    }
}