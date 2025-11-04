using Volo.Abp;
using Volo.Abp.Features;

namespace Secyud.Abp.Identities.Features;

public static class IdentitiesTwoFactorBehaviourFeatureHelper
{
    public static async Task<IdentitiesTwoFactorBehaviour> Get(IFeatureChecker featureChecker)
    {
        Check.NotNull(featureChecker, nameof(featureChecker));

        var value = await featureChecker.GetOrNullAsync(IdentitiesFeature.TwoFactor);
        if (value.IsNullOrWhiteSpace() || !Enum.TryParse<IdentitiesTwoFactorBehaviour>(value, out var behaviour))
        {
            throw new AbpException($"{IdentitiesFeature.TwoFactor} feature value is invalid");
        }

        return behaviour;
    }
}
