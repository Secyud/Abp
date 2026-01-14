using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Bundling.Styles;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Minify;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Secits.AspNetCore.Components.Bundling;

public class StyleBundler(
    IMinifier minifier,
    IOptions<AbpBundlingOptions> bundlingOptions,
    IBundleFileProvider bundleFileProvider)
    : BundlerBase(minifier, bundlingOptions), IStyleBundler
{
    protected override IFileInfo FindFileInfo(string file)
    {
        return bundleFileProvider.GetFileInfo(file);
    }

    public override string FileExtension => "css";
    
}