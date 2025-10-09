using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Secyud.Abp.Tenants;

public class AbpTenantsTestDataBuilder : ITransientDependency
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantManager _tenantManager;

    public AbpTenantsTestDataBuilder(
        ITenantRepository tenantRepository,
        ITenantManager tenantManager)
    {
        _tenantRepository = tenantRepository;
        _tenantManager = tenantManager;
    }

    public void Build()
    {
        AsyncHelper.RunSync(AddTenantsAsync);
    }

    private async Task AddTenantsAsync()
    {
        var acme = await _tenantManager.CreateAsync("acme");
        acme.ConnectionStrings.Add(new TenantConnectionString(acme.Id, ConnectionStrings.DefaultConnectionStringName, "DefaultConnString-Value"));
        acme.ConnectionStrings.Add(new TenantConnectionString(acme.Id, "MyConnString", "MyConnString-Value"));
        await _tenantRepository.InsertAsync(acme);

        var secyud = await _tenantManager.CreateAsync("secyud");
        await _tenantRepository.InsertAsync(secyud);

        var abp = await _tenantManager.CreateAsync("abp");
        await _tenantRepository.InsertAsync(abp);
    }
}
