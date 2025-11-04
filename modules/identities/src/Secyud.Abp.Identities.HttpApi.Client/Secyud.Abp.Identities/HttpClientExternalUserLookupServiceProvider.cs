using Secyud.Abp.Identities.Integration;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

[Dependency(TryRegister = true)]
public class HttpClientExternalUserLookupServiceProvider(IIdentityUserIntegrationService identityUserIntegrationService)
    : IExternalUserLookupServiceProvider, ITransientDependency
{
    protected IIdentityUserIntegrationService IdentityUserIntegrationService { get; } = identityUserIntegrationService;

    public virtual async Task<IUserData?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await IdentityUserIntegrationService.FindByIdAsync(id);
    }

    public virtual async Task<IUserData?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await IdentityUserIntegrationService.FindByUserNameAsync(userName);
    }

    public async Task<List<IUserData>> SearchAsync(
        string? sorting = null,
        string? filter = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var result = await IdentityUserIntegrationService.SearchAsync(
            new UserLookupSearchInputDto
            {
                Sorting = sorting,
                Filter = filter,
                MaxResultCount = maxResultCount,
                SkipCount = skipCount
            }
        );

        return result.Items.Cast<IUserData>().ToList();
    }

    public async Task<long> GetCountAsync(
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        return await IdentityUserIntegrationService
            .GetCountAsync(
                new UserLookupCountInputDto
                {
                    Filter = filter
                }
            );
    }
}
