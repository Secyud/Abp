using AutoMapper;

namespace Secyud.Abp.Tenants;

public class AbpTenantsBlazorAutoMapperProfile : Profile
{
    public AbpTenantsBlazorAutoMapperProfile()
    {
        CreateMap<TenantDto, TenantUpdateDto>()
            .MapExtraProperties();
    }
}
