using Shouldly;
using Volo.Abp;
using Xunit;

namespace Secyud.Abp.Tenants;

public class TenantValidatorTests : AbpTenantsDomainTestBase
{
    private readonly TenantManager _tenantManager;
    private readonly ITenantRepository _tenantRepository;

    public TenantValidatorTests()
    {
        _tenantManager = GetRequiredService<TenantManager>();
        _tenantRepository = GetRequiredService<ITenantRepository>();
    }

    [Fact]
    public async Task Should_Throw_If_Name_Is_Null()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _tenantManager.CreateAsync(""));
    }

    [Fact]
    public async Task Should_Throw_If_Duplicate_Name()
    {
        await Assert.ThrowsAsync<BusinessException>(() => _tenantManager.CreateAsync("secyud"));

        var tenant = await _tenantRepository.FindByNameAsync("ABP")!;
        await Assert.ThrowsAsync<BusinessException>(() => _tenantManager.ChangeNameAsync(tenant!, "secyud"));
    }

    [Fact]
    public async Task Should_Not_Throw_For_Unique_Name()
    {
        var tenant = await _tenantManager.CreateAsync("secyud2");
        await _tenantRepository.InsertAsync(tenant);

        tenant = await _tenantRepository.FindByNameAsync("ABP");
        await _tenantManager.ChangeNameAsync(tenant!, "secyud3");
        await _tenantRepository.UpdateAsync(tenant!);

        tenant = await _tenantRepository.FindByNameAsync("SECYUD3");
        tenant.ShouldNotBeNull();
    }
}
