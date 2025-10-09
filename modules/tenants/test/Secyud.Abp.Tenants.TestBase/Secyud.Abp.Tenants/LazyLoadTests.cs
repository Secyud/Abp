using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Secyud.Abp.Tenants;

public abstract class LazyLoadTests<TStartupModule> : TenantsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    public ITenantRepository TenantRepository { get; }
    public ITenantNormalizer TenantNormalizer { get; }

    protected LazyLoadTests()
    {
        TenantRepository = GetRequiredService<ITenantRepository>();
        TenantNormalizer = GetRequiredService<ITenantNormalizer>();
    }

    [Fact]
    public async Task Should_Lazy_Load_Tenant_Collections()
    {
        using var uow = GetRequiredService<IUnitOfWorkManager>().Begin();
        var role = await TenantRepository.FindByNameAsync(TenantNormalizer.NormalizeName("acme")!, includeDetails: false);
        role!.ConnectionStrings.ShouldNotBeNull();
        role.ConnectionStrings.Any().ShouldBeTrue();

        await uow.CompleteAsync();
    }
}
