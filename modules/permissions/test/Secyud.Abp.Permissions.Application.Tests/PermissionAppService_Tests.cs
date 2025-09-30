using System.Security.Claims;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Secyud.Abp.Permissions;

public class PermissionAppService_Tests : AbpPermissionsApplicationTestBase
{
    private readonly IPermissionAppService _permissionAppService;
    private readonly IPermissionGrantRepository _permissionGrantRepository;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public PermissionAppService_Tests()
    {
        _permissionAppService = GetRequiredService<IPermissionAppService>();
        _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
    }

    [Fact]
    public async Task GetAsync()
    {
        var resultList = await _permissionAppService.GetListAsync("TestGroup",
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());

        resultList.ShouldNotBeNull();

        resultList.ShouldContain(x => x.Name == "MyPermission1");
        resultList.ShouldContain(x => x.Name == "MyPermission2");
        resultList.ShouldContain(x => x.Name == "MyPermission3");
        resultList.ShouldContain(x => x.Name == "MyPermission4");

        resultList.ShouldNotContain(x => x.Name == "MyPermission5");
    }

    [Fact]
    public async Task GetByGroupAsync()
    {
        var resultList = await _permissionAppService.GetGroupsAsync();
        
        resultList.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task UpdateAsync()
    {
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
            "Test")).ShouldBeNull();

        await _permissionAppService.UpdateAsync("Test",
            "Test", new UpdatePermissionsDto()
            {
                Permissions =
                [
                    new UpdatePermissionDto
                    {
                        IsGranted = true,
                        Name = "MyPermission1"
                    }
                ]
            });

        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
            "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task Update_Revoke_Test()
    {
        await _permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                Guid.NewGuid(),
                "MyPermission1",
                "Test",
                "Test"
            )
        );
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
            "Test")).ShouldNotBeNull();

        await _permissionAppService.UpdateAsync("Test",
            "Test", new UpdatePermissionsDto
            {
                Permissions =
                [
                    new UpdatePermissionDto
                    {
                        IsGranted = false,
                        Name = "MyPermission1"
                    }
                ]
            });

        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
            "Test")).ShouldBeNull();
    }
}