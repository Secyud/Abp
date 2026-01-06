using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Secyud.Secits.Blazor.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Secyud.Abp.Secits.AspNetCore.Components.Bundling;

public static class SecitsBundlingExtensions
{
    extension(BundleConfigurationContext context)
    {
        public void AddSecitsExtendScripts()
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<SecitsOptions>>();
            foreach (var script in options.Value.ExtendScripts)
            {
                context.Files.AddIfNotContains(script.EnsureStartsWith('/'));
            }
        }

        public void AddSecitsExtendStyles()
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<SecitsOptions>>();
            foreach (var script in options.Value.ExtendStyles)
            {
                context.Files.AddIfNotContains(script.EnsureStartsWith('/'));
            }
        }
    }
}