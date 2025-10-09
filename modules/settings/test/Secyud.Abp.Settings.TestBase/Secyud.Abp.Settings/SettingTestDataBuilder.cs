using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public class SettingTestDataBuilder(
    ISettingRepository settingRepository,
    IGuidGenerator guidGenerator,
    SettingTestData testData)
    : ITransientDependency
{
    private readonly ISettingRepository _settingRepository = settingRepository;

    public async Task BuildAsync()
    {
        await _settingRepository.InsertAsync(
            new Setting(
                testData.SettingId,
                "MySetting1",
                "42",
                GlobalSettingValueProvider.ProviderName
            )
        );

        await _settingRepository.InsertAsync(
            new Setting(
                guidGenerator.Create(),
                "MySetting2",
                "default-store-value",
                GlobalSettingValueProvider.ProviderName
            )
        );

        await _settingRepository.InsertAsync(
            new Setting(
                guidGenerator.Create(),
                "MySetting2",
                "user1-store-value",
                UserSettingValueProvider.ProviderName,
                testData.User1Id.ToString()
            )
        );

        await _settingRepository.InsertAsync(
            new Setting(
                guidGenerator.Create(),
                "MySetting2",
                "user2-store-value",
                UserSettingValueProvider.ProviderName,
                testData.User2Id.ToString()
            )
        );

        await _settingRepository.InsertAsync(
            new Setting(
                guidGenerator.Create(),
                "MySettingWithoutInherit",
                "default-store-value",
                GlobalSettingValueProvider.ProviderName
            )
        );

        await _settingRepository.InsertAsync(
            new Setting(
                guidGenerator.Create(),
                "MySettingWithoutInherit",
                "user1-store-value",
                UserSettingValueProvider.ProviderName,
                testData.User1Id.ToString()
            )
        );
    }
}
