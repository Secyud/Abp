using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Secyud.Secits.Blazor.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Secyud.Abp.AspNetCore.Bundling;

public class BlazorSecitsThemeStyleContributor : BundleContributor
{
    private const string RootPath = "/_content/Secyud.Abp.AspNetCore.Components.Web.SecitsTheme";

    public override Task ConfigureBundleAsync(BundleConfigurationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<SecitsOptions>>();
        foreach (var style in options.Value.ExtendStyles)
        {
            context.Files.AddIfNotContains(style.EnsureStartsWith('/'));
        }
        context.Files.AddIfNotContains(RootPath + "/secits-theme.css");
        return Task.CompletedTask;
    }
}