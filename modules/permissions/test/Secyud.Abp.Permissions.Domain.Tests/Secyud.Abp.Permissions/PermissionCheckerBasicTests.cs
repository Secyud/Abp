using Shouldly;
using Xunit;

namespace Secyud.Abp.Permissions;

public class PermissionCheckerBasicTests : PermissionTestBase
{
    private readonly IPermissionChecker _permissionChecker;

    public PermissionCheckerBasicTests()
    {
        _permissionChecker = GetRequiredService<IPermissionChecker>();
    }

    [Fact]
    public async Task Should_Return_Prohibited_If_Permission_Is_Not_Defined()
    {
        (await _permissionChecker.IsGrantedAsync("UndefinedPermissionName")).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Return_False_As_Default_For_Any_Permission()
    {
        (await _permissionChecker.IsGrantedAsync("MyPermission1")).ShouldBeFalse();
    }
}
