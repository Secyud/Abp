using Volo.Abp.Security.Claims;

namespace Secyud.Abp.Identities.AspNetCore;

public class AbpRefreshingPrincipalOptions
{
    public List<string> CurrentPrincipalKeepClaimTypes { get; set; } = [AbpClaimTypes.SessionId];
}
