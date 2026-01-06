using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Secyud.Abp.Secits.AspNetCore.Components.Localization;
using Secyud.Abp.Secits.AspNetCore.Components.Theming;
using Volo.Abp.Localization;

namespace Secyud.Abp.Secits.AspNetCore.Components.Toolbars;

public partial class GeneralSettings
{
    [Inject] protected IOptions<SecitsThemeOptions> ThemeOptions { get; set; } = null!;

    [Inject] protected ISecitsThemeManager ThemeManager { get; set; } = null!;

    [Inject] protected ILanguagePlatformManager LanguagePlatformManager { get; set; } = null!;

    [Inject] protected IStringLocalizer<SecitsResource> L { get; set; } = null!;

    [Inject] protected ILanguageProvider LanguageProvider { get; set; } = null!;

    [Inject] protected IStringLocalizerFactory LocalizerFactory { get; set; } = null!;

    protected IReadOnlyList<LanguageInfo> Languages { get; set; } = new List<LanguageInfo>();
    protected LanguageInfo? CurrentLanguage { get; set; }
    protected string? CurrentLanguageTwoLetters { get; set; }
    protected string? CurrentTheme { get; set; }
    protected bool ContextMenuVisible { get; set; }
    protected bool LanguageMenuVisible { get; set; } = true;
    protected bool StyleMenuVisible { get; set; } = true;

    protected List<KeyValuePair<string, SecitsThemeStyle>> Styles => field ??= ThemeOptions.Value.Styles
        .OrderBy(u => u.Value.Order).ToList();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Languages = await LanguageProvider.GetLanguagesAsync();
            CurrentLanguage = await LanguagePlatformManager.GetCurrentAsync();

            if (CurrentLanguage is not null && !CurrentLanguage.CultureName.IsNullOrWhiteSpace())
            {
                CurrentLanguageTwoLetters = new CultureInfo(CurrentLanguage.CultureName)
                    .TwoLetterISOLanguageName.ToUpperInvariant();
            }

            CurrentTheme = await ThemeManager.GetCurrentThemeAsync();

            await InvokeAsync(StateHasChanged);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected virtual async Task ChangeLanguageAsync(LanguageInfo language)
    {
        await LanguagePlatformManager.ChangeAsync(language);
    }

    protected virtual async Task ChangeThemeAsync(string themeName)
    {
        CurrentTheme = themeName;
        await ThemeManager.SetCurrentThemeAsync(themeName);
    }

    private async Task StyleMenuIconClick()
    {
        if (ContextMenuVisible && StyleMenuVisible)
        {
            ContextMenuVisible = false;
        }
        else
        {
            ContextMenuVisible = true;
            StyleMenuVisible = true;
        }

        await Task.CompletedTask;
    }

    private async Task LanguageMenuIconClick()
    {
        if (ContextMenuVisible && LanguageMenuVisible)
        {
            ContextMenuVisible = false;
        }
        else
        {
            ContextMenuVisible = true;
            LanguageMenuVisible = true;
        }

        await Task.CompletedTask;
    }

    private async Task StyleMenuClick()
    {
        StyleMenuVisible = !StyleMenuVisible;
        await Task.CompletedTask;
    }

    private async Task LanguageMenuClick()
    {
        LanguageMenuVisible = !LanguageMenuVisible;
        await Task.CompletedTask;
    }
}