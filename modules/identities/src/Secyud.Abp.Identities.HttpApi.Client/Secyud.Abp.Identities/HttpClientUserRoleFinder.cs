using Secyud.Abp.Identities.Integration;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Identities;

[Dependency(TryRegister = true)]
public class HttpClientUserRoleFinder(IIdentityUserAppService userAppService, IIdentityUserIntegrationService userIntegrationService)
    : IUserRoleFinder, ITransientDependency
{
    protected IIdentityUserAppService UserAppService { get; } = userAppService;
    protected IIdentityUserIntegrationService UserIntegrationService { get; } = userIntegrationService;

    [Obsolete("Use GetRoleNamesAsync instead.")]
    public virtual async Task<string[]> GetRolesAsync(Guid userId)
    {
        var output = await UserAppService.GetRolesAsync(userId);
        return output.Items.Select(r => r.Name!).ToArray();
    }

    public async Task<string[]> GetRoleNamesAsync(Guid userId)
    {
        return await UserIntegrationService.GetRoleNamesAsync(userId);
    }
}