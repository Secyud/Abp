using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Secyud.Abp.Features;

public class FeatureValueCacheItemInvalidator_Tests : FeaturesTestBase<AbpFeaturesDomainTestModule>
{
    private IDistributedCache<FeatureValueCacheItem, FeatureValueCacheKey> _cache;
    private IFeatureValueRepository _featureValueRepository;
    private IFeaturesStore _featuresStore;
    private ICurrentTenant _currentTenant;

    public FeatureValueCacheItemInvalidator_Tests()
    {
        _cache = GetRequiredService<IDistributedCache<FeatureValueCacheItem, FeatureValueCacheKey>>();
        _featureValueRepository = GetRequiredService<IFeatureValueRepository>();
        _featuresStore = GetRequiredService<IFeaturesStore>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public async Task Cache_Should_Invalidator_WhenFeatureChanged()
    {
        var cacheKey = new FeatureValueCacheKey(
            TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString());
        // Arrange cache feature.
        (await _featuresStore.GetOrNullAsync(
                    TestFeatureDefinitionProvider.SocialLogins,
                    EditionFeatureValueProvider.ProviderName,
                    TestEditionIds.Regular.ToString()
                )
            ).ShouldNotBeNull();

        var feature = await _featureValueRepository.FindAsync(
            TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString()
        );

        // Act
        await _featureValueRepository.DeleteAsync(feature!);

        // Assert
        (await _cache.GetAsync(cacheKey)).ShouldBeNull();
    }


    [Fact]
    public async Task Cache_Should_Invalidator_WhenSettingChanged_Between_Tenant_And_Host()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            // Arrange cache feature.
            (await _featuresStore.GetOrNullAsync(
                        TestFeatureDefinitionProvider.SocialLogins,
                        EditionFeatureValueProvider.ProviderName,
                        TestEditionIds.Regular.ToString()
                    )
                ).ShouldNotBeNull();
        }

        using (_currentTenant.Change(null))
        {
            await _featuresStore.SetAsync(TestFeatureDefinitionProvider.SocialLogins,
                false.ToString(),
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString());
        }

        using (_currentTenant.Change(tenantId))
        {
            // Arrange cache feature.
            (await _cache.GetAsync(new FeatureValueCacheKey(
                        TestFeatureDefinitionProvider.SocialLogins,
                        EditionFeatureValueProvider.ProviderName,
                        TestEditionIds.Regular.ToString())
                    )
                ).ShouldBeNull();
        }
    }
}