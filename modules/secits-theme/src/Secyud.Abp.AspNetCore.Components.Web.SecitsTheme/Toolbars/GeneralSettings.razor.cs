using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Secyud.Abp.AspNetCore.Localization;
using Secyud.Abp.AspNetCore.Styles;
using Secyud.Secits.Blazor;
using Secyud.Secits.Blazor.Icons;
using Secyud.Secits.Blazor.Services;
using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore.Toolbars;

public partial class GeneralSettings
{
    [Inject]
    protected IOptions<SecitsThemeOptions> ThemeOptions { get; set; } = null!;

    [Inject]
    protected ISecitsStyleProvider StyleProvider { get; set; } = null!;

    [Inject]
    protected ILanguagePlatformManager LanguagePlatformManager { get; set; } = null!;

    [Inject]
    protected IStringLocalizer<SecitsResource> L { get; set; } = null!;

    [Inject]
    protected ILanguageProvider LanguageProvider { get; set; } = null!;

    [Inject]
    protected ISecitsService SecitsService { get; set; } = null!;

    [Inject]
    protected IStringLocalizerFactory LocalizerFactory { get; set; } = null!;

    protected IReadOnlyList<LanguageInfo> Languages { get; set; } = new List<LanguageInfo>();
    protected LanguageInfo? CurrentLanguage { get; set; }
    protected string? CurrentLanguageTwoLetters { get; set; }
    protected string? CurrentStyle { get; set; }
    protected bool ContextMenuVisible { get; set; }
    protected bool LanguageMenuVisible { get; set; } = true;
    protected bool StyleMenuVisible { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        Languages = await LanguageProvider.GetLanguagesAsync();
        CurrentLanguage = await LanguagePlatformManager.GetCurrentAsync();

        if (CurrentLanguage is not null && !CurrentLanguage.CultureName.IsNullOrWhiteSpace())
        {
            CurrentLanguageTwoLetters = new CultureInfo(CurrentLanguage.CultureName)
                .TwoLetterISOLanguageName.ToUpperInvariant();
        }

        CurrentStyle = await StyleProvider.GetCurrentStyleAsync();
    }

    protected virtual async Task ChangeLanguageAsync(LanguageInfo language)
    {
        await LanguagePlatformManager.ChangeAsync(language);
    }

    protected virtual async Task ChangeThemeAsync(string style)
    {
        CurrentStyle = style;

        var options = ThemeOptions.Value;
        var option = new SecitsThemeParam();
        options.Styles.GetValueOrDefault(style)?.MapTo(option);
        await SecitsService.SetCurrentStyle(style, option);
    }

    private Task StyleMenuIconClick()
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

        return Task.CompletedTask;
    }

    private Task LanguageMenuIconClick()
    {
        if (ContextMenuVisible && LanguageMenuVisible)
        {
            ContextMenuVisible = false;
        }
        else
        {
            ContextMenuVisible = true;
            StyleMenuVisible = true;
        }

        return Task.CompletedTask;
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