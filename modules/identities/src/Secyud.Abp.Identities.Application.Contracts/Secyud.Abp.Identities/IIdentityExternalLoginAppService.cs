using Volo.Abp.Application.Services;

namespace Secyud.Abp.Identities;

public interface IIdentityExternalLoginAppService : IApplicationService
{
    Task CreateOrUpdateAsync();
}