namespace Secyud.Abp.Permissions;

public class StaticPermissionSaverTests : PermissionTestBase
{
    private readonly IStaticPermissionSaver _saver;

    public StaticPermissionSaverTests()
    {
        _saver = GetRequiredService<IStaticPermissionSaver>();
    }


}
