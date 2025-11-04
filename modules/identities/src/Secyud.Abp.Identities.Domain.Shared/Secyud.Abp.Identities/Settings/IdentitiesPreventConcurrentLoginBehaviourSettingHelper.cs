using Volo.Abp;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities.Settings;

public static class IdentitiesPreventConcurrentLoginBehaviourSettingHelper
{
    public static async Task<IdentitiesPreventConcurrentLoginBehaviour> Get(ISettingProvider settingProvider)
    {
        Check.NotNull(settingProvider, nameof(settingProvider));

        var value = await settingProvider.GetOrNullAsync(IdentitiesSettingNames.Session.PreventConcurrentLogin);
        if (value.IsNullOrWhiteSpace() || !Enum.TryParse<IdentitiesPreventConcurrentLoginBehaviour>(value, out var behaviour))
        {
            throw new AbpException($"{IdentitiesSettingNames.Session.PreventConcurrentLogin} setting value is invalid");
        }

        return behaviour;
    }
}
