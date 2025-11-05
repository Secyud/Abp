using Volo.Abp.Application.Services;

namespace Secyud.Abp.Accounts.ExternalProviders;

public interface IAccountExternalProviderAppService : IApplicationService
{
    Task<ExternalProviderDto> GetAllAsync();

    Task<ExternalProviderItemWithSecretDto> GetByNameAsync(GetByNameInput input);
}
