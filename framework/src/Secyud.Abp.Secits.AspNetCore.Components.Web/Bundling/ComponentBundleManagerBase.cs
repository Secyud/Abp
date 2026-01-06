using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Bundling.Scripts;
using Volo.Abp.AspNetCore.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Secits.AspNetCore.Components.Bundling;

public abstract class ComponentBundleManagerBase(
    IOptions<AbpBundlingOptions> options,
    IOptions<AbpBundleContributorOptions> contributorOptions,
    IScriptBundler scriptBundler,
    IStyleBundler styleBundler,
    IServiceProvider serviceProvider,
    IDynamicFileProvider dynamicFileProvider,
    IBundleCache bundleCache)
    : BundleManagerBase(options,
        contributorOptions,
        scriptBundler,
        styleBundler,
        serviceProvider,
        dynamicFileProvider,
        bundleCache), ITransientDependency
{
    public override bool IsBundlingEnabled()
    {
        return Options.Mode switch
        {
            BundlingMode.None => false,
            BundlingMode.Bundle or BundlingMode.BundleAndMinify => true,
            BundlingMode.Auto => AutoEnabled(),
            _ => throw new AbpException($"Unhandled {nameof(BundlingMode)}: {Options.Mode}")
        };
    }

    protected override bool IsMinficationEnabled()
    {
        return Options.Mode switch
        {
            BundlingMode.None or BundlingMode.Bundle => false,
            BundlingMode.BundleAndMinify => true,
            BundlingMode.Auto => AutoEnabled(),
            _ => throw new AbpException($"Unhandled {nameof(BundlingMode)}: {Options.Mode}")
        };
    }

    protected abstract bool AutoEnabled();
}