using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp.Users;

namespace Secyud.Abp.Permissions;

public class AbpPermissionsApplicationTestBase : PermissionsTestBase<AbpPermissionsApplicationTestModule>
{
    protected Guid? CurrentUserId { get; set; }

    protected AbpPermissionsApplicationTestBase()
    {
        CurrentUserId = Guid.NewGuid();
    }
    protected override void AfterAddApplication(IServiceCollection services)
    {
        var currentUser = Substitute.For<ICurrentUser>();
        currentUser.Roles.Returns(new[] { "admin" });
        currentUser.IsAuthenticated.Returns(true);

        services.AddSingleton(currentUser);
    }
}
