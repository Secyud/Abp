using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Secyud.Abp.Tenants;

public class TenantConfigurationCacheItemInvalidator_Tests : AbpTenantsDomainTestBase
{
    private readonly IDistributedCache<TenantConfigurationCacheItem> _cache;
    private readonly ITenantStore _tenantStore;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantManager _tenantManager;
    private readonly ITenantNormalizer _tenantNormalizer;

    public TenantConfigurationCacheItemInvalidator_Tests()
    {
        _cache = GetRequiredService<IDistributedCache<TenantConfigurationCacheItem>>();
        _tenantStore = GetRequiredService<ITenantStore>();
        _tenantRepository = GetRequiredService<ITenantRepository>();
        _tenantManager = GetRequiredService<ITenantManager>();
        _tenantNormalizer = GetRequiredService<ITenantNormalizer>();
    }

    [Fact]
    public async Task Get_Tenant_Should_Cached()
    {
        var acme = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("acme")!);
        acme.ShouldNotBeNull();

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.NormalizedName))).ShouldBeNull();

        await _tenantStore.FindAsync(acme.Id);
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldNotBeNull();

        await _tenantStore.FindAsync(acme.NormalizedName!);
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.NormalizedName))).ShouldNotBeNull();


        var secyud = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("secyud")!);
        secyud.ShouldNotBeNull();

        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(secyud.Id, null))).ShouldBeNull();
        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(null, secyud.NormalizedName))).ShouldBeNull();

        _tenantStore.Find(secyud.Id);
        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(secyud.Id, null))).ShouldNotBeNull();

        _tenantStore.Find(secyud.NormalizedName!);
        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(null, secyud.NormalizedName))).ShouldNotBeNull();
    }

    [Fact]
    public async Task Cache_Should_Invalidator_When_Tenant_Changed()
    {
        var acme = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("acme")!);
        acme.ShouldNotBeNull();

        // FindAsync will cache tenant.
        await _tenantStore.FindAsync(acme.Id);
        await _tenantStore.FindAsync(acme.NormalizedName!);

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldNotBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.NormalizedName))).ShouldNotBeNull();

        await _tenantRepository.DeleteAsync(acme);

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.NormalizedName))).ShouldBeNull();


        var secyud = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("secyud")!);
        secyud.ShouldNotBeNull();

        // Find will cache tenant.
        _tenantStore.Find(secyud.Id);
        _tenantStore.Find(secyud.NormalizedName!);

        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(secyud.Id, null))).ShouldNotBeNull();
        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(null, secyud.NormalizedName))).ShouldNotBeNull();

        await _tenantRepository.DeleteAsync(secyud);

        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(secyud.Id, null))).ShouldBeNull();
        (_cache.Get(TenantConfigurationCacheItem.CalculateCacheKey(null, secyud.NormalizedName))).ShouldBeNull();

        var abp = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("abp")!);
        abp.ShouldNotBeNull();

        // Find will cache tenant.
        await _tenantStore.FindAsync(abp.Id);
        await _tenantStore.FindAsync(abp.NormalizedName!);

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(abp.Id, null))).ShouldNotBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, abp.NormalizedName))).ShouldNotBeNull();

        await _tenantManager.ChangeNameAsync(abp, "abp2");
        await _tenantRepository.UpdateAsync(abp);

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(abp.Id, null))).ShouldBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, _tenantNormalizer.NormalizeName("abp")))).ShouldBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, _tenantNormalizer.NormalizeName("abp2")))).ShouldBeNull();
    }
}
