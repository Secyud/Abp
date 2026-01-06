using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.RequestLocalization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Secyud.Abp.Secits.AspNetCore.Components.Localization;

[Dependency(ReplaceServices = true)]
public class BlazorServerLanguagePlatformManager(
    NavigationManager navigationManager,
    ILanguageProvider languageProvider,
    IAbpRequestLocalizationOptionsProvider requestLocalizationOptionsProvider)
    : ILanguagePlatformManager, ITransientDependency
{
    protected NavigationManager NavigationManager { get; } = navigationManager;
    protected ILanguageProvider LanguageProvider { get; } = languageProvider;
    protected IAbpRequestLocalizationOptionsProvider RequestLocalizationOptionsProvider { get; } = requestLocalizationOptionsProvider;

    public virtual Task ChangeAsync(LanguageInfo newLanguage)
    {
        var relativeUrl = NavigationManager.Uri.RemovePreFix(NavigationManager.BaseUri).EnsureStartsWith('/');

        NavigationManager.NavigateTo(
            $"Abp/Languages/Switch?culture={newLanguage.CultureName}&uiCulture={newLanguage.UiCultureName}&returnUrl={relativeUrl}",
            forceLoad: true
        );

        return Task.CompletedTask;
    }

    public virtual async Task<LanguageInfo> GetCurrentAsync()
    {
        var languages = await LanguageProvider.GetLanguagesAsync();
        var currentLanguage = languages.FindByCulture(
            CultureInfo.CurrentCulture.Name,
            CultureInfo.CurrentUICulture.Name
        );

        if (currentLanguage is null)
        {
            var localizationOptions = await RequestLocalizationOptionsProvider.GetLocalizationOptionsAsync();

            currentLanguage = new LanguageInfo(
                localizationOptions.DefaultRequestCulture.Culture.Name,
                localizationOptions.DefaultRequestCulture.UICulture.Name,
                localizationOptions.DefaultRequestCulture.UICulture.DisplayName);
        }

        return currentLanguage;
    }
}