
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Secyud.Abp.AspNetCore.Bundling
{
    public class BlazorSecitsThemeStyleContributor : BundleContributor
    {
        private const string RootPath = "/_content/Secyud.Abp.AspNetCore.Components.Web.SecitsTheme";

        public override Task ConfigureBundleAsync(BundleConfigurationContext context)
        {

            return Task.CompletedTask;
        }
    }
}