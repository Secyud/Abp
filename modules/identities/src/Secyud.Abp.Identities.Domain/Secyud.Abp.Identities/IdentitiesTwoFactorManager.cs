using Secyud.Abp.Identities.Features;
using Secyud.Abp.Identities.Settings;
using Volo.Abp.Domain.Services;
using Volo.Abp.Features;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities;

public class IdentitiesTwoFactorManager(IFeatureChecker featureChecker, ISettingProvider settingProvider) : IDomainService
{
    protected IFeatureChecker FeatureChecker { get; } = featureChecker;

    protected ISettingProvider SettingProvider { get; } = settingProvider;

    public virtual async Task<bool> IsOptionalAsync()
    {
        var feature = await IdentitiesTwoFactorBehaviourFeatureHelper.Get(FeatureChecker);
        if (feature == IdentitiesTwoFactorBehaviour.Optional)
        {
            var setting = await IdentitiesTwoFactorBehaviourSettingHelper.Get(SettingProvider);
            if (setting == IdentitiesTwoFactorBehaviour.Optional)
            {
                return true;
            }
        }

        return false;
    }

    public virtual async Task<bool> IsForcedEnableAsync()
    {
        var feature = await IdentitiesTwoFactorBehaviourFeatureHelper.Get(FeatureChecker);
        if (feature == IdentitiesTwoFactorBehaviour.Forced)
        {
            return true;
        }

        var setting = await IdentitiesTwoFactorBehaviourSettingHelper.Get(SettingProvider);
        if (setting == IdentitiesTwoFactorBehaviour.Forced)
        {
            return true;
        }

        return false;
    }

    public virtual async Task<bool> IsForcedDisableAsync()
    {
        var feature = await IdentitiesTwoFactorBehaviourFeatureHelper.Get(FeatureChecker);
        if (feature == IdentitiesTwoFactorBehaviour.Disabled)
        {
            return true;
        }

        var setting = await IdentitiesTwoFactorBehaviourSettingHelper.Get(SettingProvider);
        if (setting == IdentitiesTwoFactorBehaviour.Disabled)
        {
            return true;
        }
        return false;
    }
}
