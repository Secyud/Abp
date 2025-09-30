namespace Secyud.Abp.Settings;

public class SettingsComponentOptions
{
    public List<ISettingComponentContributor> Contributors { get; }

    public SettingsComponentOptions()
    {
        Contributors = new List<ISettingComponentContributor>();
    }
}
