using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Xunit;

namespace Secyud.Abp.Permissions;

public class PermissionDataSeedContributor_Tests : PermissionTestBase
{
    private readonly PermissionDataSeedContributor _permissionDataSeedContributor;
    private readonly IPermissionGrantRepository _grantpermissionGrantRepository;

    public PermissionDataSeedContributor_Tests()
    {
        _permissionDataSeedContributor = GetRequiredService<PermissionDataSeedContributor>();
        _grantpermissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
    }

    [Fact]
    public async Task SeedAsync()
    {
        (await _grantpermissionGrantRepository.FindAsync("MyPermission1", RolePermissionValueProvider.ProviderName, "admin")).ShouldBeNull();
        (await _grantpermissionGrantRepository.FindAsync("MyPermission4", RolePermissionValueProvider.ProviderName, "admin")).ShouldBeNull();

        await _permissionDataSeedContributor.SeedAsync(new DataSeedContext(null));

        (await _grantpermissionGrantRepository.FindAsync("MyPermission1", RolePermissionValueProvider.ProviderName, "admin")).ShouldNotBeNull();
        (await _grantpermissionGrantRepository.FindAsync("MyPermission4", RolePermissionValueProvider.ProviderName, "admin")).ShouldBeNull();
    }
}
