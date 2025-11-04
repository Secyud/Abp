using Secyud.Abp.Identities.Features;
using Volo.Abp;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities.Settings;

public static class IdentitiesTwoFactorBehaviourSettingHelper
{
    public static async Task<IdentitiesTwoFactorBehaviour> Get(ISettingProvider settingProvider)
    {
        Check.NotNull(settingProvider, nameof(settingProvider));

        var value = await settingProvider.GetOrNullAsync(IdentitiesSettingNames.TwoFactor.Behaviour);
        if (value.IsNullOrWhiteSpace() || !Enum.TryParse<IdentitiesTwoFactorBehaviour>(value, out var behaviour))
        {
            throw new AbpException($"{IdentitiesSettingNames.TwoFactor.Behaviour} setting value is invalid");
        }

        return behaviour;
    }
}
