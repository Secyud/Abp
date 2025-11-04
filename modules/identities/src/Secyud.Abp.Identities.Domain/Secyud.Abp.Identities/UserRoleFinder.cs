using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Identities;

public class UserRoleFinder(IIdentityUserRepository identityUserRepository) : IUserRoleFinder, ITransientDependency
{
    protected IIdentityUserRepository IdentityUserRepository { get; } = identityUserRepository;

    [Obsolete("Use GetRoleNamesAsync instead.")]
    public virtual async Task<string[]> GetRolesAsync(Guid userId)
    {
        return (await IdentityUserRepository.GetRoleNamesAsync(userId)).ToArray();
    }

    public async Task<string[]> GetRoleNamesAsync(Guid userId)
    {
        return (await IdentityUserRepository.GetRoleNamesAsync(userId)).ToArray();
    }
}
