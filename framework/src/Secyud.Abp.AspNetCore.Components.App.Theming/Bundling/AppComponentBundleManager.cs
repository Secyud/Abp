using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.AspNetCore.Components.Bundling;

public class AppComponentBundleManager(
    IOptions<AbpBundlingOptions> options,
    IServiceProvider serviceProvider,
    IFileProvider fileProvider)
    : IComponentBundleManager, ITransientDependency
{
    private readonly AbpBundlingOptions _options = options.Value;

    public virtual async Task<IReadOnlyList<string>> GetStyleBundleFilesAsync(string bundleName)
    {
        return (await GetBundleFilesAsync(_options.StyleBundles, bundleName)).Select(f => f.FileName).ToList();
    }

    public virtual async Task<IReadOnlyList<string>> GetScriptBundleFilesAsync(string bundleName)
    {
        return (await GetBundleFilesAsync(_options.ScriptBundles, bundleName)).Select(f => f.FileName).ToList();
    }

    private async Task<List<BundleFile>> GetBundleFilesAsync(BundleConfigurationCollection collection,
        string bundleName)
    {
        var queue = new Queue<BundleConfiguration>();
        queue.Enqueue(collection.Get(bundleName));
        var contributors = new List<IBundleContributor>();

        while (queue.Count > 0)
        {
            var bundle = queue.Dequeue();
            contributors.AddRange(bundle.Contributors.GetAll());
            foreach (var baseName in bundle.BaseBundles)
            {
                queue.Enqueue(collection.Get(baseName));
            }
        }

        contributors.Reverse();

        var context = new BundleConfigurationContext(serviceProvider, fileProvider);
        foreach (var contributor in contributors)
        {
            await contributor.PreConfigureBundleAsync(context);
        }

        foreach (var contributor in contributors)
        {
            await contributor.ConfigureBundleAsync(context);
        }

        foreach (var contributor in contributors)
        {
            await contributor.PostConfigureBundleAsync(context);
        }

        return context.Files;
    }
}