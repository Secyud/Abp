using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Accounts;

public interface IIdentityLinkUserAppService : IApplicationService
{
    Task<ListResultDto<LinkUserDto>> GetAllListAsync();

    Task LinkAsync(LinkUserInput input);

    Task UnlinkAsync(UnLinkUserInput input);

    Task<bool> IsLinkedAsync(IsLinkedInput input);

    Task<string> GenerateLinkTokenAsync();

    Task<bool> VerifyLinkTokenAsync(VerifyLinkTokenInput input);

    Task<string> GenerateLinkLoginTokenAsync();

    Task<bool> VerifyLinkLoginTokenAsync(VerifyLinkLoginTokenInput input);
}
