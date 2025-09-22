using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.AspNetCore.Styles;

public class SecitsStyleProvider(
    IHttpContextAccessor httpContextAccessor,
    IOptions<SecitsThemeOptions> secitsThemeOption)
    : ISecitsStyleProvider, IScopedDependency
{
    protected const string SecitsStyleCookieName = "secits_loaded-css";

    protected SecitsThemeOptions SecitsThemeOption { get; } = secitsThemeOption.Value;

    protected string? CurrentStyle { get; set; }

    public event EventHandler? StyleChanged;

    public virtual async Task<string> GetCurrentStyleAsync()
    {
        await Task.CompletedTask;
        if (CurrentStyle is null)
        {
            var styleName = httpContextAccessor.HttpContext?.Request.Cookies[SecitsStyleCookieName];

            if (string.IsNullOrWhiteSpace(styleName) || !SecitsThemeOption.Styles.ContainsKey(styleName))
            {
                return SecitsThemeOption.DefaultStyle == SecitsStyleNames.System
                    ? SecitsStyleNames.Default
                    : SecitsThemeOption.DefaultStyle;
            }

            CurrentStyle = styleName;
        }

        return CurrentStyle;
    }

    public async Task SetCurrentStyleAsync(string styleName)
    {
        await Task.CompletedTask;
        CurrentStyle = styleName;
        StyleChanged?.Invoke(this, EventArgs.Empty);
    }
}