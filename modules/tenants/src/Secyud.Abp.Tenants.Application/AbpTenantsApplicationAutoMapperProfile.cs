using AutoMapper;

namespace Secyud.Abp.Tenants;

public class AbpTenantsApplicationAutoMapperProfile : Profile
{
    public AbpTenantsApplicationAutoMapperProfile()
    {
        CreateMap<Tenant, TenantDto>()
            .MapExtraProperties();
    }
}
