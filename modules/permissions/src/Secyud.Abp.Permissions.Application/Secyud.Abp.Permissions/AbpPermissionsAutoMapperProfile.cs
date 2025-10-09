using AutoMapper;

namespace Secyud.Abp.Permissions;

public class AbpPermissionsAutoMapperProfile : Profile
{
    public AbpPermissionsAutoMapperProfile()
    {
        CreateMap<PermissionGroupInfo, PermissionGroupInfoDto>();
        CreateMap<PermissionGrantInfo, PermissionGrantInfoDto>();
    }
}