using System.Globalization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore.Navigation;

[ExposeServices(typeof(ILanguagePlatformManager))]
public class AppLanguagePlatformManager(
    ILanguageProvider languageProvider,
    ICurrentLanguageProvider currentLanguageProvider,
    NavigationManager navigationManager)
    : ILanguagePlatformManager, ITransientDependency
{
    protected ILanguageProvider LanguageProvider { get; } = languageProvider;
    protected ICurrentLanguageProvider CurrentLanguageProvider { get; } = currentLanguageProvider;

    public virtual async Task ChangeAsync(LanguageInfo newLanguage)
    {
        var culture = CultureInfo.GetCultureInfo(newLanguage.CultureName);
        var uiCulture = CultureInfo.GetCultureInfo(newLanguage.UiCultureName);

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = uiCulture;
        CultureHelper.Use(culture);

        await CurrentLanguageProvider.SetCurrentLanguageAsync(newLanguage);

        navigationManager.NavigateTo(navigationManager.Uri, true);
    }

    public virtual async Task<LanguageInfo> GetCurrentAsync()
    {
        var currentLanguage = await CurrentLanguageProvider.GetCurrentLanguageAsync();
        if (currentLanguage is not null) return currentLanguage;

        var languages = await LanguageProvider.GetLanguagesAsync();
        currentLanguage = languages.FirstOrDefault(u =>
            string.Equals(u.TwoLetterISOLanguageName,
                CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
        ) ?? languages[0];

        return currentLanguage;
    }
}