using Shouldly;
using Volo.Abp.Caching;
using Xunit;

namespace Secyud.Abp.Permissions;

public class PermissionGrantCacheItemInvalidatorTests : PermissionTestBase
{
    private readonly IDistributedCache<PermissionGrantCacheItem, PermissionGrantCacheKey> _cache;
    private readonly IPermissionStore _permissionStore;
    private readonly IPermissionGrantRepository _permissionGrantRepository;

    private readonly PermissionGrantCacheKey _cacheKey = new("MyPermission1",
        UserPermissionValueProvider.ProviderName,
        PermissionTestDataBuilder.User1Id.ToString());

    public PermissionGrantCacheItemInvalidatorTests()
    {
        _cache = GetRequiredService<IDistributedCache<PermissionGrantCacheItem, PermissionGrantCacheKey>>();
        _permissionStore = GetRequiredService<IPermissionStore>();
        _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
    }

    [Fact]
    public async Task PermissionStore_IsGrantedAsync_Should_Cache_PermissionGrant()
    {
        (await _cache.GetAsync(_cacheKey)).ShouldBeNull();

        await _permissionStore.IsGrantedAsync("MyPermission1",
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());


        (await _cache.GetAsync(_cacheKey)).ShouldNotBeNull();
    }

    [Fact]
    public async Task Cache_Should_Invalidator_WhenPermissionGrantChanged()
    {
        // IsGrantedAsync will cache PermissionGrant
        await _permissionStore.IsGrantedAsync("MyPermission1",
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());

        var permissionGrant = await _permissionGrantRepository.FindAsync("MyPermission1",
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());
        permissionGrant.ShouldNotBeNull();
        await _permissionGrantRepository.DeleteAsync(permissionGrant);

        (await _cache.GetAsync(_cacheKey)).ShouldBeNull();
    }
}