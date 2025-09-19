using Volo.Abp.Reflection;

namespace SecitsDemoApp.Permissions;

public class SecitsDemoAppPermissions
{
    public const string GroupName = "SecitsDemoApp";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(SecitsDemoAppPermissions));
    }
}
