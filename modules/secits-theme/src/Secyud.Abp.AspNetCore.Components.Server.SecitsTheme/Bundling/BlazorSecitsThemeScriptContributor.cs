using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Secyud.Secits.Blazor.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Secyud.Abp.AspNetCore.Bundling;

public class BlazorSecitsThemeScriptContributor : BundleContributor
{
    private const string RootPath = "/_content/Secyud.Abp.AspNetCore.Components.Web.SecitsTheme";
    private const string ServerRootPath = "/_content/Secyud.Abp.AspNetCore.Components.Server.SecitsTheme";

    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<SecitsOptions>>();
        foreach (var script in options.Value.ExtendScripts)
        {
            context.Files.AddIfNotContains(script.EnsureStartsWith('/'));
        }

        context.Files.Add(RootPath + "/secits-theme.js");
    }
}