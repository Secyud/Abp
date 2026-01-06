using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Secyud.Abp.Secits.AspNetCore.Components.Bundling;

public class BlazorWebAssemblyStyleContributor : BundleContributor
{
    private const string SecitsRootPath = "/_content/Secyud.Abp.Secits.AspNetCore.Components.Web";
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.AddSecitsExtendStyles();
        context.Files.AddIfNotContains("/_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/css/abp.css");
        context.Files.AddIfNotContains("/_content/Secyud.Abp.Secits.Blazor/secyud.abp.secits.css");
        context.Files.AddIfNotContains(SecitsRootPath + "/flag-icon/css/flag-icon.min.css");
        context.Files.AddIfNotContains(SecitsRootPath + "/secits-theme.css");
    }
}