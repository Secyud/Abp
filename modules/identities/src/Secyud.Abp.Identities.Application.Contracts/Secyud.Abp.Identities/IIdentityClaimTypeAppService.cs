using Volo.Abp.Application.Services;

namespace Secyud.Abp.Identities;

public interface IIdentityClaimTypeAppService
    : ICrudAppService<ClaimTypeDto, Guid, GetIdentityClaimTypesInput, CreateClaimTypeDto, UpdateClaimTypeDto>
{

}
