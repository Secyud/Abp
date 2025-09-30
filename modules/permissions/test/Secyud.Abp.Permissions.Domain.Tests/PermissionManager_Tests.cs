using Shouldly;
using Volo.Abp;
using Xunit;

namespace Secyud.Abp.Permissions;

public class PermissionManager_Tests : PermissionTestBase
{
    private readonly IPermissionManager _permissionManager;
    private readonly IPermissionGrantRepository _permissionGrantRepository;
    private readonly IPermissionDefinitionRecordRepository _permissionDefinitionRecordRepository;

    public PermissionManager_Tests()
    {
        _permissionManager = GetRequiredService<IPermissionManager>();
        _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        _permissionDefinitionRecordRepository = GetRequiredService<IPermissionDefinitionRecordRepository>();
    }

    [Fact]
    public async Task GetAsync()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );

        var grantedProviders = await _permissionManager.GetAsync("MyPermission1",
            "Test",
            "Test");

        grantedProviders.ShouldNotBeNull();
        grantedProviders.IsGranted.ShouldBeTrue();
        grantedProviders.Name.ShouldBe("MyPermission1");
        grantedProviders.Providers!.ShouldContain(x => x == "Test");
    }


    [Fact]
    public async Task Get_Should_Return_Not_Granted_When_Permission_Undefined()
    {
        var result = await _permissionManager.GetAsync("MyPermission1NotExist", "Test", "Test");
        result.Name.ShouldBe("MyPermission1NotExist");
        result.Providers.ShouldBeEmpty();
        result.IsGranted.ShouldBeFalse();
    }

    [Fact]
    public async Task GetAllAsync()
    {
        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission1",
            "Test",
            "Test")
        );

        await _permissionGrantRepository.InsertAsync(new PermissionGrant(
            Guid.NewGuid(),
            "MyPermission2",
            "Test",
            "Test")
        );

        var permissionWithGrantedProviders = await _permissionManager.GetListAsync(
            "TestGroup", "Test", "Test");

        permissionWithGrantedProviders.ShouldNotBeNull();
        permissionWithGrantedProviders.ShouldContain(x =>
            x.IsGranted && x.Name == "MyPermission1" && x.Providers != null && x.Providers.Any(p => p == "Test"));
        permissionWithGrantedProviders.ShouldContain(x =>
            x.IsGranted && x.Name == "MyPermission2" && x.Providers != null && x.Providers.Any(p => p == "Test"));
    }

    [Fact]
    public async Task UpdateAsync()
    {
        (await _permissionGrantRepository.FindAsync("MyPermission2",
            "Test",
            "Test")).ShouldBeNull();

        await _permissionManager.UpdateAsync(
            "Test",
            "Test", ["MyPermission2"], []);

        (await _permissionGrantRepository.FindAsync("MyPermission2",
            "Test",
            "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task Set_Should_Silently_Ignore_When_Permission_Undefined()
    {
        await _permissionManager.UpdateAsync(
            "Test",
            "Test", ["MyPermission1NotExist"], []);
    }

    [Fact]
    public async Task Set_Should_Throw_Exception_If_Provider_Not_Found()
    {
        var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
        {
            await _permissionManager.UpdateAsync(
                "UndefinedProvider", "Test",
                ["MyPermission1NotExist"], []);
        });

        exception.Message.ShouldBe("Unknown permission value provider: UndefinedProvider");
    }
}