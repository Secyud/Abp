namespace Secyud.Abp.Identities.Settings;

public enum IdentitiesPreventConcurrentLoginBehaviour
{
    Disabled = 0,

    LogoutFromSameTypeDevices = 1,

    LogoutFromAllDevices = 2
}
