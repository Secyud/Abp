using Volo.Abp.Collections;

namespace Secyud.Abp.Settings;

public class SettingsOptions
{
    public ITypeList<ISettingsProvider> Providers { get; } = new TypeList<ISettingsProvider>();

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveStaticSettingsToDatabase { get; set; } = true;

    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsDynamicSettingStoreEnabled { get; set; }
}