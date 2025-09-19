using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.AspNetCore;

public class SecitsStyleProvider(IHttpContextAccessor httpContextAccessor, IOptions<SecitsThemeOptions> secitsThemeOption)
    : ITransientDependency
{
    private const string SecitsStyleCookieName = "secits_loaded-css";

    private readonly SecitsThemeOptions _secitsThemeOption = secitsThemeOption.Value;

    public virtual Task<string> GetSelectedStyleAsync()
    {
        var styleName = httpContextAccessor.HttpContext?.Request.Cookies[SecitsStyleCookieName];

        if (string.IsNullOrWhiteSpace(styleName) || !_secitsThemeOption.Styles.ContainsKey(styleName))
        {
            if (_secitsThemeOption.DefaultStyle == SecitsStyleNames.System)
            {
                return Task.FromResult(SecitsStyleNames.Default);
            }

            return Task.FromResult(_secitsThemeOption.DefaultStyle);
        }

        return Task.FromResult(styleName);
    }
}