using AutoMapper;
using Secyud.Abp.Permissions.Components;

namespace Secyud.Abp.Permissions;

public class AbpPermissionsBlazorAutoMapperProfile : Profile
{
    public AbpPermissionsBlazorAutoMapperProfile()
    {
        CreateMap<PermissionGrantInfoDto, PermissionGrantInfoModel>()
            .ForMember(u => u.IsChecked, v
                => v.MapFrom(u => u.IsGranted));
    }
}