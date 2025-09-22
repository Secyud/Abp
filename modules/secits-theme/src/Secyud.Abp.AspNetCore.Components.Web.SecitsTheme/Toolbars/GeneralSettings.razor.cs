using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Secyud.Abp.AspNetCore.Localization;
using Secyud.Abp.AspNetCore.Styles;
using Secyud.Secits.Blazor.Icons;
using Volo.Abp.Localization;

namespace Secyud.Abp.AspNetCore.Toolbars;

public partial class GeneralSettings
{
    [Inject]
    protected IIconProvider IconProvider { get; set; } = null!;

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
    protected IStringLocalizerFactory LocalizerFactory { get; set; } = null!;

    protected IReadOnlyList<LanguageInfo> Languages { get; set; } = new List<LanguageInfo>();
    protected LanguageInfo? CurrentLanguage { get; set; }
    protected string? CurrentLanguageTwoLetters { get; set; }
    protected string? CurrentStyle { get; set; }
    protected bool ContextMenuVisible { get; set; }
    protected bool LanguageMenuVisible { get; set; }
    protected bool StyleMenuVisible { get; set; }
    protected string? LanguageMenuIcon { get; set; }
    protected string? StyleMenuIcon { get; set; }
    protected string? UpIcon { get; set; }
    protected string? DownIcon { get; set; }

    protected override async Task OnInitializedAsync()
    {
        UpIcon = IconProvider.GetIcon(IconName.UpAngle);
        DownIcon = IconProvider.GetIcon(IconName.DownAngle);
        LanguageMenuIcon = IconProvider.GetIcon(IconName.Globe);
        StyleMenuIcon = IconProvider.GetIcon(IconName.Palette);
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
        await StyleProvider.SetCurrentStyleAsync(style);
    }

    private void StyleMenuIconClick()
    {
        if (ContextMenuVisible) ContextMenuVisible = false;
        else
        {
            ContextMenuVisible = true;
            StyleMenuVisible = true;
        }
    }

    private void LanguageMenuIconClick()
    {
        if (ContextMenuVisible) ContextMenuVisible = false;
        else
        {
            ContextMenuVisible = true;
            LanguageMenuVisible = true;
        }
    }

    private void StyleMenuClick()
    {
        StyleMenuVisible = !StyleMenuVisible;
    }

    private void LanguageMenuClick()
    {
        LanguageMenuVisible = !LanguageMenuVisible;
    }
}