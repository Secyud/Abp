using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Secyud.Secits.Blazor.Options;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.AspNetCore.Styles;

public class SecitsStyleProvider(
    IHttpContextAccessor httpContextAccessor,
    IOptions<SecitsThemeOptions> secitsThemeOption)
    : ISecitsStyleProvider, ITransientDependency
{
    protected const string SecitsStyleCookieName = SecitsStylesOptions.CookieName;

    protected SecitsThemeOptions SecitsThemeOption { get; } = secitsThemeOption.Value;

    public virtual Task<string> GetCurrentStyleAsync()
    {
        var styleName = httpContextAccessor.HttpContext?.Request.Cookies[SecitsStyleCookieName];

        if (string.IsNullOrWhiteSpace(styleName) || !SecitsThemeOption.Styles.ContainsKey(styleName))
        {
            styleName = SecitsThemeOption.DefaultStyle == SecitsStyleNames.System
                ? SecitsStyleNames.Default
                : SecitsThemeOption.DefaultStyle;
        }

        return Task.FromResult(styleName);
    }
}