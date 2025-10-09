using Volo.Abp.Reflection;

namespace Secyud.Abp.Features;

public class FeaturesPermissions
{
    public const string GroupName = "Features";

    public const string ManageHostFeatures = GroupName + ".ManageHostFeatures";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FeaturesPermissions));
    }
}
