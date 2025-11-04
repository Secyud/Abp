using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities.Integration;

public class IdentityUserIntegrationService(
    IIdentityUserRepository userRepository,
    IdentityUserRepositoryExternalUserLookupServiceProvider userLookupServiceProvider)
    : IdentityAppServiceBase, IIdentityUserIntegrationService
{
    protected IIdentityUserRepository UserRepository { get; } = userRepository;

    protected IdentityUserRepositoryExternalUserLookupServiceProvider UserLookupServiceProvider { get; } = userLookupServiceProvider;

    public async Task<string[]> GetRoleNamesAsync(Guid id)
    {
        return (await UserRepository.GetRoleNamesAsync(id)).ToArray();
    }

    public async Task<UserData?> FindByIdAsync(Guid id)
    {
        var userData = await UserLookupServiceProvider.FindByIdAsync(id);
        if (userData == null)
        {
            return null;
        }

        return new UserData(userData);
    }

    public async Task<UserData?> FindByUserNameAsync(string userName)
    {
        var userData = await UserLookupServiceProvider.FindByUserNameAsync(userName);
        if (userData == null)
        {
            return null;
        }

        return new UserData(userData);
    }

    public async Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
    {
        var users = await UserLookupServiceProvider.SearchAsync(
            input.Sorting,
            input.Filter,
            input.MaxResultCount,
            input.SkipCount
        );

        return new ListResultDto<UserData>(
            users
                .Select(u => new UserData(u))
                .ToList()
        );
    }

    public async Task<long> GetCountAsync(UserLookupCountInputDto input)
    {
        return await UserLookupServiceProvider.GetCountAsync(input.Filter);
    }
}