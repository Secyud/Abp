using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Secyud.Abp.Identities;

public abstract class LazyLoadingTests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IIdentityUserRepository UserRepository { get; }
    protected IIdentityRoleRepository RoleRepository { get; }
    protected ILookupNormalizer LookupNormalizer { get; }

    protected LazyLoadingTests()
    {
        UserRepository = ServiceProvider.GetRequiredService<IIdentityUserRepository>();
        RoleRepository = ServiceProvider.GetRequiredService<IIdentityRoleRepository>();
        LookupNormalizer = ServiceProvider.GetRequiredService<ILookupNormalizer>();
    }

    [Fact]
    public async Task Should_Lazy_Load_Role_Collections()
    {
        using var uow = GetRequiredService<IUnitOfWorkManager>().Begin();
        var role = await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName("moderator"), includeDetails: false);
        role!.Claims.ShouldNotBeNull();
        role.Claims.Any().ShouldBeTrue();

        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Should_Lazy_Load_User_Collections()
    {
        using var uow = GetRequiredService<IUnitOfWorkManager>().Begin();
        var john = await UserRepository.FindByNormalizedUserNameAsync(LookupNormalizer.NormalizeName("john.nash"), includeDetails: false);

        john!.Roles.ShouldNotBeNull();
        john.Roles.Any().ShouldBeTrue();

        john.Logins.ShouldNotBeNull();
        john.Logins.Any().ShouldBeTrue();

        john.Claims.ShouldNotBeNull();
        john.Claims.Any().ShouldBeTrue();

        john.Tokens.ShouldNotBeNull();
        john.Tokens.Any().ShouldBeTrue();

        await uow.CompleteAsync();
    }
}
