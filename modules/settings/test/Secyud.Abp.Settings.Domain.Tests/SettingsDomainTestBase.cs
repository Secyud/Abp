using Volo.Abp.Modularity;

namespace Secyud.Abp.Settings;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class SettingsDomainTestBase<TStartupModule> : SettingsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
