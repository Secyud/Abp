using AutoMapper;
using Secyud.Abp.Accounts.Pages.Account;

namespace Secyud.Abp.Accounts;

public class AbpAccountBlazorAutoMapperProfile : Profile
{
    public AbpAccountBlazorAutoMapperProfile()
    {
        CreateMap<ProfileDto, PersonalInfoModel>()
            .MapExtraProperties();

        CreateMap<PersonalInfoModel, UpdateProfileDto>()
            .MapExtraProperties();
    }
}
