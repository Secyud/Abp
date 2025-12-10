using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.AspNetCore.Styles;

public class DefaultSecitsStyleProvider(
    IOptions<SecitsThemeOptions> secitsThemeOption)
    : ISecitsStyleProvider, ISingletonDependency
{
    private readonly string _styleName = null!;

    protected SecitsThemeOptions SecitsThemeOption { get; } = secitsThemeOption.Value;

    public virtual Task<string> GetCurrentStyleAsync()
    {
        var styleName = _styleName;

        if (string.IsNullOrWhiteSpace(styleName) || !SecitsThemeOption.Styles.ContainsKey(styleName))
        {
            styleName = SecitsThemeOption.DefaultStyle == SecitsStyleNames.System
                ? SecitsStyleNames.Default
                : SecitsThemeOption.DefaultStyle;
        }

        return Task.FromResult(styleName);
    }
}