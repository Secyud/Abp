using Shouldly;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Xunit;

namespace Secyud.Abp.Settings;

public class SettingsStore_Tests : SettingsTestBase
{
    private readonly ISettingsStore _SettingsStore;
    private readonly ISettingRepository _settingRepository;
    private readonly SettingTestData _testData;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public SettingsStore_Tests()
    {
        _SettingsStore = GetRequiredService<ISettingsStore>();
        _settingRepository = GetRequiredService<ISettingRepository>();
        _testData = GetRequiredService<SettingTestData>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task GetOrNull_NotExist_Should_Be_Null()
    {
        var value = await _SettingsStore.GetOrNullAsync("notExistName", "notExistProviderName",
            "notExistProviderKey");

        value.ShouldBeNull();
    }

    [Fact]
    public async Task GetOrNullAsync()
    {
        var value = await _SettingsStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);

        value.ShouldNotBeNull();
        value.ShouldBe("42");
    }

    [Fact]
    public async Task SetAsync()
    {
        var setting = await _settingRepository.FindAsync(_testData.SettingId);
        setting!.Value.ShouldBe("42");

        await _SettingsStore.SetAsync("MySetting1", "43", GlobalSettingValueProvider.ProviderName, null);

        (await _settingRepository.FindAsync(_testData.SettingId))!.Value.ShouldBe("43");
    }

    [Fact]
    public async Task Set_In_UnitOfWork_Should_Be_Consistent()
    {
        using (_unitOfWorkManager.Begin())
        {
            var value = await _SettingsStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);
            value.ShouldBe("42");

            await _SettingsStore.SetAsync("MySetting1", "43", GlobalSettingValueProvider.ProviderName, null);

            var valueAfterSet = await _SettingsStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);
            valueAfterSet.ShouldBe("43");
        }
    }

    [Fact]
    public async Task DeleteAsync()
    {
        (await _settingRepository.FindAsync(_testData.SettingId)).ShouldNotBeNull();

        await _SettingsStore.DeleteAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);

        (await _settingRepository.FindAsync(_testData.SettingId)).ShouldBeNull();
    }

    [Fact]
    public async Task Delete_In_UnitOfWork_Should_Be_Consistent()
    {
        using var uow = _unitOfWorkManager.Begin();
        (await _SettingsStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null)).ShouldNotBeNull();

        await _SettingsStore.DeleteAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);

        await uow.SaveChangesAsync();

        var value = await _SettingsStore.GetOrNullAsync("MySetting1", GlobalSettingValueProvider.ProviderName, null);
        value.ShouldBeNull();
    }

    [Fact]
    public async Task GetListAsync()
    {
        var result = await _SettingsStore.GetListAsync(
            new[]
            {
                    "MySetting1",
                    "MySetting2",
                    "MySetting3",
                    "notExistName"
            },
            GlobalSettingValueProvider.ProviderName,
            null);

        result.Count.ShouldBe(4);

        result.First(x => x.Name == "MySetting1").Value.ShouldBe("42");
        result.First(x => x.Name == "MySetting2").Value.ShouldBe("default-store-value");
        result.First(x => x.Name == "MySetting3").Value.ShouldBe(null);
        result.First(x => x.Name == "notExistName").Value.ShouldBe(null);
    }
}
