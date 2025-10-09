using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Xunit;

namespace Secyud.Abp.Settings;

public class SettingCacheItemInvalidatorTests : SettingsDomainTestBase
{
    private readonly IDistributedCache<SettingCacheItem, SettingCacheKey> _cache;
    private readonly ISettingsStore _settingsStore;
    private readonly ISettingRepository _settingRepository;
    private readonly SettingTestData _testData;
    private readonly ICurrentTenant _currentTenant;

    public SettingCacheItemInvalidatorTests()
    {
        _settingsStore = GetRequiredService<ISettingsStore>();
        _cache = GetRequiredService<IDistributedCache<SettingCacheItem, SettingCacheKey>>();
        _settingRepository = GetRequiredService<ISettingRepository>();
        _testData = GetRequiredService<SettingTestData>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task GetOrNullAsync_Should_Cached()
    {
        var cacheKey = new SettingCacheKey("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString());

        // Act
        (await _cache.GetAsync(cacheKey)).ShouldBeNull();
        await _settingsStore.GetOrNullAsync("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString());
        (await _cache.GetAsync(cacheKey)).ShouldNotBeNull();
    }

    [Fact]
    public async Task Cache_Should_Invalidator_WhenSettingChanged()
    {
        var cacheKey = new SettingCacheKey("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString());
        // Arrange
        // GetOrNullAsync will cache language.
        await _settingsStore.GetOrNullAsync("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString());

        // Act
        var lang = await _settingRepository.FindAsync("MySetting2", UserSettingValueProvider.ProviderName, _testData.User1Id.ToString());
        await _settingRepository.DeleteAsync(lang!);

        // Assert
        (await _cache.GetAsync(cacheKey)).ShouldBeNull();
    }

    [Fact]
    public async Task Cache_Should_Invalidator_WhenSettingChanged_Between_Tenant_And_Host()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            // GetOrNullAsync will cache language.
            await _settingsStore
                    .GetOrNullAsync("MySetting2", GlobalSettingValueProvider.ProviderName, null)
                ;
        }

        using (_currentTenant.Change(null))
        {
            // SetAsync will make cache invalid.
            await _settingsStore
                .SetAsync("MySetting2", "MySetting2Value", GlobalSettingValueProvider.ProviderName, null);
        }

        using (_currentTenant.Change(tenantId))
        {
            var cacheKey = new SettingCacheKey("MySetting2", UserSettingValueProvider.ProviderName, null);
            // Assert
            (await _cache.GetAsync(cacheKey)).ShouldBeNull();
        }
    }
}