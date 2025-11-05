using Volo.Abp.Application.Services;

namespace Secyud.Abp.Accounts;

public interface IAccountExternalLoginAppService : IApplicationService
{
    Task<List<AccountExternalLoginDto>> GetListAsync();

    Task DeleteAsync(string loginProvider, string providerKey);

    Task<bool> HasPasswordVerifiedAsync(Guid userId, string loginProvider, string providerKey);

    Task SetPasswordVerifiedAsync(string loginProvider, string providerKey);

    Task RemovePasswordVerifiedAsync(string loginProvider, string providerKey);
}
