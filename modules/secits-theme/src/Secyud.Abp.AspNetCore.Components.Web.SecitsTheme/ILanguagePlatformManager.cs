using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore;

public interface ILanguagePlatformManager
{
    Task ChangeAsync(LanguageInfo newLanguage);

    Task<LanguageInfo> GetCurrentAsync();
}