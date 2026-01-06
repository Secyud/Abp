using Volo.Abp.Localization;

namespace Secyud.Abp.Secits.AspNetCore.Components.Localization;

public interface ILanguagePlatformManager
{
    Task ChangeAsync(LanguageInfo newLanguage);

    Task<LanguageInfo> GetCurrentAsync();
}