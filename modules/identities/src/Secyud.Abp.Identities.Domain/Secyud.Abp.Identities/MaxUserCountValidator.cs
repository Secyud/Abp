using Microsoft.AspNetCore.Identity;
using Secyud.Abp.Identities.Features;
using Volo.Abp;
using Volo.Abp.Features;

namespace Secyud.Abp.Identities;

public class MaxUserCountValidator(IFeatureChecker featureChecker, IIdentityUserRepository userRepository) : IUserValidator<IdentityUser>
{
    protected IFeatureChecker FeatureChecker { get; } = featureChecker;
    protected IIdentityUserRepository UserRepository { get; } = userRepository;

    public async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
    {
        await CheckMaxUserCountAsync(user);

        return IdentityResult.Success;
    }

    protected virtual async Task CheckMaxUserCountAsync(IdentityUser user)
    {
        var maxUserCount = await FeatureChecker.GetAsync<int>(IdentitiesFeature.MaxUserCount);
        if (maxUserCount <= 0)
        {
            return;
        }

        var existUser = await UserRepository.FindAsync(user.Id);
        if (existUser == null)
        {
            var currentUserCount = await UserRepository.GetCountAsync();
            if (currentUserCount >= maxUserCount)
            {
                throw new BusinessException(IdentitiesErrorCodes.MaximumUserCount)
                    .WithData("MaxUserCount", maxUserCount);
            }
        }
    }
}
