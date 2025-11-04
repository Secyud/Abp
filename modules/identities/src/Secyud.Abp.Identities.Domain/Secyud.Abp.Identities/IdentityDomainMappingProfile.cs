using AutoMapper;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

public class IdentityDomainMappingProfile : Profile
{
    public IdentityDomainMappingProfile()
    {
        CreateMap<IdentityUser, UserEto>();
        CreateMap<IdentityClaimType, IdentityClaimTypeEto>();
        CreateMap<IdentityRole, IdentityRoleEto>();
    }
}
