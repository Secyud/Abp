using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Secyud.Abp.Tenants;

public class AbpTenantsTestDataBuilder(
    ITenantRepository tenantRepository,
    ITenantManager tenantManager) : ITransientDependency
{
    public void Build()
    {
        AsyncHelper.RunSync(AddTenantsAsync);
    }

    private async Task AddTenantsAsync()
    {
        var acme = await tenantManager.CreateAsync("acme");
        acme.ConnectionStrings.Add(new TenantConnectionString(acme.Id, ConnectionStrings.DefaultConnectionStringName, "DefaultConnString-Value"));
        acme.ConnectionStrings.Add(new TenantConnectionString(acme.Id, "MyConnString", "MyConnString-Value"));
        await tenantRepository.InsertAsync(acme);

        var secyud = await tenantManager.CreateAsync("secyud");
        await tenantRepository.InsertAsync(secyud);

        var abp = await tenantManager.CreateAsync("abp");
        await tenantRepository.InsertAsync(abp);
    }
}
