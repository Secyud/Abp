using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Secyud.Abp.Identities.Features;
using Secyud.Abp.Identities.Settings;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.Settings;

namespace Secyud.Abp.Identities;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IdentitiesUserStore), typeof(IdentityUserStore))]
public class IdentitiesUserStore(
    IIdentityUserRepository userRepository,
    IIdentityRoleRepository roleRepository,
    IGuidGenerator guidGenerator,
    ILogger<IdentityRoleStore> logger,
    ILookupNormalizer lookupNormalizer,
    IFeatureChecker featureChecker,
    ISettingProvider settingProvider,
    IdentityErrorDescriber? describer = null)
    : IdentityUserStore(userRepository,
        roleRepository,
        guidGenerator,
        logger,
        lookupNormalizer,
        describer)
{
    protected IFeatureChecker FeatureChecker { get; } = featureChecker;
    protected ISettingProvider SettingProvider { get; } = settingProvider;

    public override async Task<bool> GetTwoFactorEnabledAsync(IdentityUser user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Check.NotNull(user, nameof(user));

        var feature = await IdentitiesTwoFactorBehaviourFeatureHelper.Get(FeatureChecker);
        if (feature == IdentitiesTwoFactorBehaviour.Disabled)
        {
            return false;
        }
        if (feature == IdentitiesTwoFactorBehaviour.Forced)
        {
            return true;
        }

        var setting = await IdentitiesTwoFactorBehaviourSettingHelper.Get(SettingProvider);
        if (setting == IdentitiesTwoFactorBehaviour.Disabled)
        {
            return false;
        }

        if (setting == IdentitiesTwoFactorBehaviour.Forced)
        {
            return true;
        }

        return user.TwoFactorEnabled;
    }
}
