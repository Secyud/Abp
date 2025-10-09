using Shouldly;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Secyud.Abp.Tenants;

public class TenantManagerTests : AbpTenantsDomainTestBase
{
    private readonly ITenantManager _tenantManager;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantNormalizer _tenantNormalizer;

    public TenantManagerTests()
    {
        _tenantManager = GetRequiredService<ITenantManager>();
        _tenantRepository = GetRequiredService<ITenantRepository>();
        _tenantNormalizer = GetRequiredService<ITenantNormalizer>();
    }

    [Fact]
    public async Task CreateAsync()
    {
        var tenant = await _tenantManager.CreateAsync("Test");
        tenant.Name.ShouldBe("Test");
        tenant.NormalizedName.ShouldBe(_tenantNormalizer.NormalizeName("Test"));
    }

    [Fact]
    public async Task Create_Tenant_Name_Can_Not_Duplicate()
    {
        await Assert.ThrowsAsync<BusinessException>(async () => await _tenantManager.CreateAsync("secyud"));
    }

    [Fact]
    public async Task ChangeNameAsync()
    {
        var tenant = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("secyud")!);
        tenant.ShouldNotBeNull();
        tenant.NormalizedName.ShouldBe(_tenantNormalizer.NormalizeName("secyud"));

        await _tenantManager.ChangeNameAsync(tenant, "newsecyud");

        tenant.Name.ShouldBe("newsecyud");
        tenant.NormalizedName.ShouldBe(_tenantNormalizer.NormalizeName("newsecyud"));
    }

    [Fact]
    public async Task ChangeName_Tenant_Name_Can_Not_Duplicate()
    {
        var tenant = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("acme")!);
        tenant.ShouldNotBeNull();

        await Assert.ThrowsAsync<BusinessException>(async () => await _tenantManager.ChangeNameAsync(tenant, "secyud"));
    }
}
