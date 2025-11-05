using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Accounts;

public interface IProfileAppService : IApplicationService
{
    Task<ProfileDto> GetAsync();

    Task<ProfileDto> UpdateAsync(UpdateProfileDto input);

    Task ChangePasswordAsync(ChangePasswordInput input);

    Task<bool> GetTwoFactorEnabledAsync();

    Task SetTwoFactorEnabledAsync(bool enabled);

    Task<bool> CanEnableTwoFactorAsync();

    Task<List<NameValue>> GetTimezonesAsync();
}
