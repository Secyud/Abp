using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Settings;

public interface ITimeZoneSettingsAppService : IApplicationService
{
    Task<string?> GetAsync();

    Task<List<NameValue>> GetTimezonesAsync();

    Task UpdateAsync(string? timezone);
}
