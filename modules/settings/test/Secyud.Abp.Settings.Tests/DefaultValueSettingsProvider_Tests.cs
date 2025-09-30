using Shouldly;
using Volo.Abp;
using Volo.Abp.Settings;
using Xunit;

namespace Secyud.Abp.Settings;

public class DefaultValueSettingsProvider_Tests : SettingsTestBase
{
    private readonly ISettingDefinitionManager _settingDefinitionManager;

    public DefaultValueSettingsProvider_Tests()
    {
        _settingDefinitionManager = GetRequiredService<ISettingDefinitionManager>();
    }

    [Fact]
    public async Task GetOrNullAsync()
    {
        var mySetting3 = await _settingDefinitionManager.GetAsync("MySetting3");

        var defaultValueSettingsProvider = new DefaultValueSettingsProvider();
        (await defaultValueSettingsProvider
            .GetOrNullAsync(mySetting3, DefaultValueSettingValueProvider.ProviderName)).ShouldBe("123");
    }

    [Fact]
    public async Task SetAsync()
    {
        var mySetting3 = await _settingDefinitionManager.GetAsync("MySetting3");

        await Assert.ThrowsAsync<AbpException>(async () => await new DefaultValueSettingsProvider().SetAsync(mySetting3, "123",
            DefaultValueSettingValueProvider.ProviderName));
    }

    [Fact]
    public async Task ClearAsync()
    {
        var mySetting3 = await _settingDefinitionManager.GetAsync("MySetting3");

        await Assert.ThrowsAsync<AbpException>(async () =>
            await new DefaultValueSettingsProvider().ClearAsync(mySetting3,
                DefaultValueSettingValueProvider.ProviderName));
    }
}
