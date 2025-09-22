using System.Globalization;
using Microsoft.JSInterop;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore;

[ExposeServices(typeof(ILanguagePlatformManager))]
public class LanguageBlazorWasmManager(
    ILanguageProvider languageProvider,
    ILocalStorageService localStorageService,
    ICookieService cookieService,
    IJSRuntime jsRuntime)
    : ILanguagePlatformManager, ITransientDependency
{
    protected const string LanguageSettingKey = "Abp.SelectedLanguage";
    protected const string IsRtlKey = "Abp.IsRtl";
    protected ILanguageProvider LanguageProvider { get; } = languageProvider;
    protected ICookieService CookieService { get; } = cookieService;
    protected ILocalStorageService LocalStorageService { get; } = localStorageService;
    protected IJSRuntime JsJsRuntime { get; } = jsRuntime;

    public virtual async Task ChangeAsync(LanguageInfo newLanguage)
    {
        CultureInfo.CurrentUICulture = new CultureInfo(newLanguage.UiCultureName);

        await LocalStorageService.SetItemAsync(LanguageSettingKey, newLanguage.UiCultureName);
        await LocalStorageService.SetItemAsync(IsRtlKey, CultureHelper.IsRtl.ToString());

        await CookieService.SetAsync(
            ".AspNetCore.Culture",
            $"c={newLanguage.CultureName}|uic={newLanguage.UiCultureName}",
            new CookieOptions
            {
                Path = "/"
            }
        );

        await JsJsRuntime.InvokeVoidAsync("location.reload");
    }

    public virtual async Task<LanguageInfo> GetCurrentAsync()
    {
        var selectedLanguageName = await LocalStorageService
            .GetItemAsync(LanguageSettingKey);

        var languages = await LanguageProvider.GetLanguagesAsync();

        if (languages.Count == 0)
        {
            return new LanguageInfo(
                CultureInfo.CurrentCulture.Name,
                CultureInfo.CurrentUICulture.Name);
        }

        var currentLanguage = languages[0];

        foreach (var language in languages)
        {
            if (language.UiCultureName == selectedLanguageName) return language;
            if (language.UiCultureName == CultureInfo.CurrentUICulture.Name)
                currentLanguage = language;
        }

        return currentLanguage;
    }
}