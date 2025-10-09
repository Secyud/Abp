using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Secyud.Abp.Permissions;

public abstract class PermissionDefinitionRecordRepositoryTests<TStartupModule> : PermissionsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IPermissionDefinitionRecordRepository PermissionDefinitionRecordRepository { get; set; }

    protected PermissionDefinitionRecordRepositoryTests()
    {
        PermissionDefinitionRecordRepository = GetRequiredService<IPermissionDefinitionRecordRepository>();
    }

    [Fact]
    public async Task FindByNameAsync()
    {
        var permission = await PermissionDefinitionRecordRepository.FindByNameAsync("MyPermission1");
        permission.ShouldNotBeNull();
        permission.Name.ShouldBe("MyPermission1");

        permission = await PermissionDefinitionRecordRepository.FindByNameAsync("MyPermission2");
        permission.ShouldNotBeNull();
        permission.Name.ShouldBe("MyPermission2");
    }
}
