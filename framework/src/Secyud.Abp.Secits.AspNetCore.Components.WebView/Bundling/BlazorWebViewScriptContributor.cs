using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Secyud.Secits.Blazor.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Secyud.Abp.Secits.AspNetCore.Components.Bundling;

public class BlazorWebViewScriptContributor : BundleContributor
{
    private const string RootPath = "/_content/Volo.Abp.AspNetCore.Components.Web";
    private const string SecitsRootPath = "/_content/Secyud.Abp.Secits.AspNetCore.Components.Web";

    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.AddSecitsExtendScripts();
        context.Files.AddIfNotContains(RootPath + "/libs/abp/js/abp.js");
        context.Files.AddIfNotContains(RootPath + "/libs/abp/js/authentication-state-listener.js");
        context.Files.AddIfNotContains(RootPath + "/libs/abp/js/lang-utils.js");
        context.Files.AddIfNotContains(SecitsRootPath + "/secits-theme.js");
    }
}