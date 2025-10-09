using Volo.Abp.Modularity;

namespace Secyud.Abp.Settings;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class SettingsApplicationTestBase<TStartupModule> : SettingsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
