using Microsoft.Extensions.Options;
using Secyud.Abp.Secits.AspNetCore.Components.Application;
using Secyud.Secits.Blazor.Options;
using Secyud.Secits.Blazor.Services;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Secits.AspNetCore.Components.Theming;

public class SecitsThemeManager(
    IWebStorage storage,
    IOptions<SecitsThemeOptions> options,
    ISecitsService secitsService
) : ISecitsThemeManager, ITransientDependency
{
    protected const string Key = SecitsStylesOptions.CookieName;
    public IWebStorage Storage { get; } = storage;
    protected ISecitsService SecitsService { get; } = secitsService;
    protected SecitsThemeOptions Options { get; } = options.Value;

    public virtual async Task<string> GetCurrentThemeAsync()
    {
        return await Storage.GetValueAsync(Key) ?? SecitsThemeNames.Default;
    }

    public virtual async Task SetCurrentThemeAsync(string style)
    {
        var input = Options.GenerateThemeInput(style);
        await SecitsService.SetCurrentStyle(style, input);
        await Storage.SetValueAsync(Key, style);
    }
}