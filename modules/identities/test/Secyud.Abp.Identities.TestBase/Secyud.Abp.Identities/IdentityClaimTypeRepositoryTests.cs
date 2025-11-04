using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Xunit;

namespace Secyud.Abp.Identities;

public abstract class IdentityClaimTypeRepositoryTests<TStartupModule> : AbpIdentityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IIdentityClaimTypeRepository ClaimTypeRepository { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public IdentityClaimTypeRepositoryTests()
    {
        ClaimTypeRepository = ServiceProvider.GetRequiredService<IIdentityClaimTypeRepository>();
        GuidGenerator = ServiceProvider.GetRequiredService<IGuidGenerator>();
    }

    [Fact]
    public async Task Should_Check_Name_If_It_Is_Uniquee()
    {
        var claim = (await ClaimTypeRepository.GetListAsync()).First();

        var result1 = await ClaimTypeRepository.AnyAsync(claim.Name);

        result1.ShouldBe(true);

        var result2 = await ClaimTypeRepository.AnyAsync(Guid.NewGuid().ToString());

        result2.ShouldBe(false);
    }
}
