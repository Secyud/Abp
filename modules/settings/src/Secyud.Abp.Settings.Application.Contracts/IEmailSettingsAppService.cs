using Volo.Abp.Application.Services;

namespace Secyud.Abp.Settings;

public interface IEmailSettingsAppService : IApplicationService
{
    Task<EmailSettingsDto> GetAsync();

    Task UpdateAsync(UpdateEmailSettingsDto input);

    Task SendTestEmailAsync(SendTestEmailInput input);
}
