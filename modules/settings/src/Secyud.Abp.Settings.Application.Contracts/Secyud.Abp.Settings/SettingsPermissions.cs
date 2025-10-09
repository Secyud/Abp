using Volo.Abp.Reflection;

namespace Secyud.Abp.Settings;

public class SettingsPermissions
{
    public const string GroupName = "Settings";

    public const string Emailing = GroupName + ".Emailing";

    public const string EmailingTest = Emailing + ".Test";

    public const string TimeZone = GroupName + ".TimeZone";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(SettingsPermissions));
    }
}
