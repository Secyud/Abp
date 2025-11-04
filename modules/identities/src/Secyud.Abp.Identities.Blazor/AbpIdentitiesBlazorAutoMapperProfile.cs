using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Secyud.Abp.Identities;

public class AbpIdentitiesBlazorAutoMapperProfile : Profile
{
    public AbpIdentitiesBlazorAutoMapperProfile()
    {
        CreateMap<IdentityUserDto, IdentityUserUpdateDto>()
            .MapExtraProperties()
            .Ignore(x => x.RoleNames);

        CreateMap<IdentityRoleDto, IdentityRoleUpdateDto>()
            .MapExtraProperties();

        CreateMap<ClaimTypeDto, UpdateClaimTypeDto>()
            .MapExtraProperties();
    }
}