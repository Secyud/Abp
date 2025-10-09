using Volo.Abp.Domain.Services;

namespace Secyud.Abp.Tenants;

public interface ITenantManager : IDomainService
{
    Task<Tenant> CreateAsync(string name);

    Task ChangeNameAsync(Tenant tenant, string name);
}
