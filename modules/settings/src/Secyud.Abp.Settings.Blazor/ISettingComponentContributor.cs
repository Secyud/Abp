namespace Secyud.Abp.Settings;

public interface ISettingComponentContributor
{
    Task ConfigureAsync(SettingComponentCreationContext context);

    Task<bool> CheckPermissionsAsync(SettingComponentCreationContext context);
}
