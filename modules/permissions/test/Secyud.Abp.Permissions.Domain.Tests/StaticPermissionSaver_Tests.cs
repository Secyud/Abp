namespace Secyud.Abp.Permissions;

public class StaticPermissionSaver_Tests : PermissionTestBase
{
    private readonly IStaticPermissionSaver _saver;

    public StaticPermissionSaver_Tests()
    {
        _saver = GetRequiredService<IStaticPermissionSaver>();
    }


}
