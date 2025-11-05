using Volo.Abp.Application.Services;

namespace Secyud.Abp.Accounts;

public interface IDynamicClaimsAppService : IApplicationService
{
    Task RefreshAsync();
}
