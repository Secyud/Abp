using Microsoft.AspNetCore.Identity;
using Secyud.Abp.Users;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

public class IdentityUserRepositoryExternalUserLookupServiceProvider(
    IIdentityUserRepository userRepository,
    ILookupNormalizer lookupNormalizer) : IExternalUserLookupServiceProvider, ITransientDependency
{
    protected IIdentityUserRepository UserRepository { get; } = userRepository;
    protected ILookupNormalizer LookupNormalizer { get; } = lookupNormalizer;

    public virtual async Task<IUserData?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return (
                await UserRepository.FindAsync(
                    id,
                    includeDetails: false,
                    cancellationToken: cancellationToken
                )
            )?.ToAbpUserData();
    }

    public virtual async Task<IUserData?> FindByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default)
    {
        return (
                await UserRepository.FindByNormalizedUserNameAsync(
                    LookupNormalizer.NormalizeName(userName),
                    includeDetails: false,
                    cancellationToken: cancellationToken
                )
            )?.ToAbpUserData();
    }

    public virtual async Task<List<IUserData>> SearchAsync(
        string? sorting = null,
        string? filter = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var users = await UserRepository.GetListAsync(
            sorting: sorting,
            maxResultCount: maxResultCount,
            skipCount: skipCount,
            filter: filter,
            includeDetails: false,
            cancellationToken: cancellationToken
        );

        return users.Select(u => u.ToAbpUserData()).ToList();
    }

    public async Task<long> GetCountAsync(
        string? filter = null,
        CancellationToken cancellationToken = new())
    {
        return await UserRepository.GetCountAsync(filter, cancellationToken: cancellationToken);
    }
}
