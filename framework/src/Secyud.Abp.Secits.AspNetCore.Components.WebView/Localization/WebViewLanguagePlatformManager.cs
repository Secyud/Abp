using System.Globalization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Secyud.Abp.Secits.AspNetCore.Components.Localization;

[Dependency(ReplaceServices = true)]
public class WebViewLanguagePlatformManager(
    ILanguageProvider languageProvider,
    NavigationManager navigationManager)
    : ILanguagePlatformManager, ITransientDependency
{
    protected ILanguageProvider LanguageProvider { get; } = languageProvider;

    public virtual Task ChangeAsync(LanguageInfo newLanguage)
    {
        var culture = CultureInfo.GetCultureInfo(newLanguage.CultureName);
        var uiCulture = CultureInfo.GetCultureInfo(newLanguage.UiCultureName);

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = uiCulture;

        navigationManager.NavigateTo(navigationManager.Uri, true);

        return Task.CompletedTask;
    }

    public virtual async Task<LanguageInfo> GetCurrentAsync()
    {
        var languages = await LanguageProvider.GetLanguagesAsync();
        var currentLanguage = languages.FirstOrDefault(u =>
            string.Equals(u.TwoLetterISOLanguageName,
                CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName)
        ) ?? languages[0];

        return currentLanguage;
    }
}