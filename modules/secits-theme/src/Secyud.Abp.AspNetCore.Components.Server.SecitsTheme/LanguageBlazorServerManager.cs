using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.RequestLocalization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore;

[ExposeServices(typeof(ILanguagePlatformManager))]
public class LanguageBlazorServerManager : ILanguagePlatformManager, ITransientDependency
{
    protected NavigationManager NavigationManager { get; }

    protected ILanguageProvider LanguageProvider { get; }

    protected IAbpRequestLocalizationOptionsProvider RequestLocalizationOptionsProvider { get; }

    public LanguageBlazorServerManager(
        NavigationManager navigationManager,
        ILanguageProvider languageProvider,
        IAbpRequestLocalizationOptionsProvider requestLocalizationOptionsProvider)
    {
        NavigationManager = navigationManager;
        LanguageProvider = languageProvider;
        RequestLocalizationOptionsProvider = requestLocalizationOptionsProvider;
    }

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