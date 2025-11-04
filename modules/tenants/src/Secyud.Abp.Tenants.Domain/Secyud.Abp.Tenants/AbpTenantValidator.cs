using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Tenants;

public class AbpTenantValidator(ITenantRepository tenantRepository) : ITenantValidator, ITransientDependency
{
    protected ITenantRepository TenantRepository { get; } = tenantRepository;

    public virtual async Task ValidateAsync(Tenant tenant)
    {
        Check.NotNullOrWhiteSpace(tenant.Name, nameof(tenant.Name));
        Check.NotNullOrWhiteSpace(tenant.NormalizedName, nameof(tenant.NormalizedName));

        var owner = await TenantRepository.FindByNameAsync(tenant.NormalizedName);
        if (owner != null && owner.Id != tenant.Id)
        {
            throw new BusinessException("Secyud.Abp.Tenants:DuplicateTenantName").WithData("Name", tenant.NormalizedName);
        }
    }
}
