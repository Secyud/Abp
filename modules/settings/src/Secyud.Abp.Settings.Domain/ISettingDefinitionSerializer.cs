using Volo.Abp.Settings;

namespace Secyud.Abp.Settings;

public interface ISettingDefinitionSerializer
{
    Task<SettingDefinitionRecord> SerializeAsync(SettingDefinition setting);

    Task<List<SettingDefinitionRecord>> SerializeAsync(IEnumerable<SettingDefinition> settings);
}
