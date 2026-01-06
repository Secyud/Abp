using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Bundling.Scripts;
using Volo.Abp.AspNetCore.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Secits.AspNetCore.Components.Bundling;

public class BlazorWebAssemblyBundleManager(
    IOptions<AbpBundlingOptions> options,
    IOptions<AbpBundleContributorOptions> contributorOptions,
    IScriptBundler scriptBundler,
    IStyleBundler styleBundler,
    IServiceProvider serviceProvider,
    IDynamicFileProvider dynamicFileProvider,
    IBundleCache bundleCache)
    : ComponentBundleManagerBase(options,
        contributorOptions,
        scriptBundler,
        styleBundler,
        serviceProvider,
        dynamicFileProvider,
        bundleCache)
{

    protected override bool AutoEnabled()
    {
        return true;
    }

    protected override IFileProvider GetFileProvider()
    {
        return null!;
    }
}