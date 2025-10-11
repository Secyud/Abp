﻿using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Features;
using Volo.Abp.Users;
using Xunit;

namespace Secyud.Abp.Features;

public class FeatureAppServiceTests : FeaturesApplicationTestBase
{
    private readonly IFeatureAppService _featureAppService;
    private ICurrentUser? _currentUser;
    private readonly FeaturesTestData _testData;


    public FeatureAppServiceTests()
    {
        _featureAppService = GetRequiredService<IFeatureAppService>();
        _testData = GetRequiredService<FeaturesTestData>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _currentUser = Substitute.For<ICurrentUser>();
        services.AddSingleton(_currentUser);
    }

    [Fact]
    public async Task GetAsync()
    {
        Login(_testData.User1Id);

        var featureList = await _featureAppService.GetAsync(EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString());

        featureList.ShouldNotBeNull();
        featureList.Groups.SelectMany(g => g.Features).ShouldContain(feature => feature.Name == TestFeatureDefinitionProvider.SocialLogins);
    }

    [Fact]
    public async Task UpdateAsync()
    {
        Login(_testData.User1Id);

        await _featureAppService.UpdateAsync(EditionFeatureValueProvider.ProviderName,
            TestEditionIds.Regular.ToString(), new UpdateFeaturesDto()
            {
                Features = new List<UpdateFeatureDto>()
                {
                        new UpdateFeatureDto()
                        {
                            Name = TestFeatureDefinitionProvider.SocialLogins,
                            Value = false.ToString().ToLowerInvariant()
                        }
                }
            });

        (await _featureAppService.GetAsync(EditionFeatureValueProvider.ProviderName,
                TestEditionIds.Regular.ToString())).Groups.SelectMany(g => g.Features).Any(x =>
                x.Name == TestFeatureDefinitionProvider.SocialLogins &&
                x.Value == false.ToString().ToLowerInvariant())
            .ShouldBeTrue();

    }

    [Fact]
    public async Task ResetToDefaultAsync()
    {
        Login(_testData.User1Id);
        var exception = await Record.ExceptionAsync(async () =>
            await _featureAppService.DeleteAsync("test", "test"));
        Assert.Null(exception);
    }

    private void Login(Guid userId)
    {
        _currentUser!.Id.Returns(userId);
        _currentUser.IsAuthenticated.Returns(true);
    }
}
