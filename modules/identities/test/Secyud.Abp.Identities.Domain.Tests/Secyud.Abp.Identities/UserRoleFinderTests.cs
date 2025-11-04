using Shouldly;
using Xunit;

namespace Secyud.Abp.Identities;

public class UserRoleFinderTests : AbpIdentityDomainTestBase
{
    private readonly IUserRoleFinder _userRoleFinder;
    private readonly IdentityTestData _testData;

    public UserRoleFinderTests()
    {
        _userRoleFinder = GetRequiredService<IUserRoleFinder>();
        _testData = GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task GetRolesAsync()
    {
        var roleNames = await _userRoleFinder.GetRoleNamesAsync(_testData.UserJohnId);
        roleNames.ShouldNotBeEmpty();
        roleNames.ShouldContain(x => x == "moderator");
        roleNames.ShouldContain(x => x == "supporter");
    }
}
