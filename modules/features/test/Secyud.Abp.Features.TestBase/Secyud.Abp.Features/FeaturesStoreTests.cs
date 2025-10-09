using Shouldly;
using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Secyud.Abp.Features;

public abstract class FeaturesStoreTests<TStartupModule> : FeaturesTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private IFeaturesStore FeaturesStore { get; set; }
    private IFeatureValueRepository FeatureValueRepository { get; set; }
    private IUnitOfWorkManager UnitOfWorkManager { get; set; }

    protected FeaturesStoreTests()
    {
        FeaturesStore = GetRequiredService<IFeaturesStore>();
        FeatureValueRepository = GetRequiredService<IFeatureValueRepository>();
        UnitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task GetOrNullAsync()
    {
        // Act
        (await FeaturesStore.GetOrNullAsync(Guid.NewGuid().ToString(),
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString())).ShouldBeNull();

        (await FeaturesStore.GetOrNullAsync(TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString())).ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Get_Null_Where_Feature_Deleted()
    {
        // Arrange
        (await FeaturesStore.GetOrNullAsync(TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString())).ShouldNotBeNull();

        // Act
        await FeaturesStore.DeleteAsync(TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString());

        // Assert
        (await FeaturesStore.GetOrNullAsync(TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString())).ShouldBeNull();
    }

    [Fact]
    public async Task SetAsync()
    {
        // Arrange
        (await FeatureValueRepository.FindAsync(TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString()))!.Value.ShouldBe(true.ToString().ToLowerInvariant());

        // Act
        await FeaturesStore.SetAsync(TestFeatureDefinitionProvider.SocialLogins,
            false.ToString().ToUpperInvariant(),
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString());

        // Assert
        (await FeatureValueRepository.FindAsync(TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString()))!.Value.ShouldBe(false.ToString().ToUpperInvariant());
    }

    [Fact]
    public async Task Set_In_UnitOfWork_Should_Be_Consistent()
    {
        using (UnitOfWorkManager.Begin())
        {
            // Arrange
            (await FeaturesStore.GetOrNullAsync(TestFeatureDefinitionProvider.SocialLogins,
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString())).ShouldNotBeNull();


            // Act
            await FeaturesStore.SetAsync(TestFeatureDefinitionProvider.SocialLogins,
                false.ToString().ToUpperInvariant(),
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString());

            // Assert
            (await FeaturesStore.GetOrNullAsync(TestFeatureDefinitionProvider.SocialLogins,
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString())).ShouldBe(false.ToString().ToUpperInvariant());
        }
    }

    [Fact]
    public async Task DeleteAsync()
    {
        // Arrange
        (await FeatureValueRepository.FindAsync(TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString())).ShouldNotBeNull();

        // Act
        await FeaturesStore.DeleteAsync(TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString());


        // Assert
        (await FeatureValueRepository.FindAsync(TestFeatureDefinitionProvider.SocialLogins,
            EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString())).ShouldBeNull();


    }

    [Fact]
    public async Task Delete_In_UnitOfWork_Should_Be_Consistent()
    {
        using (var uow = UnitOfWorkManager.Begin())
        {
            // Arrange
            (await FeaturesStore.GetOrNullAsync(TestFeatureDefinitionProvider.SocialLogins,
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString())).ShouldNotBeNull();

            // Act
            await FeaturesStore.DeleteAsync(TestFeatureDefinitionProvider.SocialLogins,
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString());

            await uow.SaveChangesAsync();

            // Assert
            (await FeaturesStore.GetOrNullAsync(TestFeatureDefinitionProvider.SocialLogins,
                EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString())).ShouldBeNull();
        }
    }
}
