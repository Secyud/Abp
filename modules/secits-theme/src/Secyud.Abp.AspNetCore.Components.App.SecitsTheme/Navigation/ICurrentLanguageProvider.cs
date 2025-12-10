using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore.Navigation;

public interface ICurrentLanguageProvider
{
    Task<LanguageInfo?> GetCurrentLanguageAsync();
    Task SetCurrentLanguageAsync(LanguageInfo? languageInfo);
}