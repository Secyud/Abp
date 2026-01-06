using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Secyud.Abp.Secits.AspNetCore.Components.Bundling;

public class BlazorServerScriptContributor : BundleContributor
{
    private const string RootPath = "/_content/Volo.Abp.AspNetCore.Components.Web";
    private const string SecitsRootPath = "/_content/Secyud.Abp.Secits.AspNetCore.Components.Web";

    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.AddSecitsExtendScripts();
        context.Files.AddIfNotContains(RootPath + "/libs/abp/js/abp.js");
        context.Files.AddIfNotContains(RootPath + "/libs/abp/js/authentication-state-listener.js");
        context.Files.AddIfNotContains(SecitsRootPath + "/secits-theme.js");
    }
}