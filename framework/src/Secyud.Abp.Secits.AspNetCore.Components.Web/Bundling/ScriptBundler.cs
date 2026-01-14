using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Bundling;
using Volo.Abp.AspNetCore.Bundling.Scripts;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Minify;

namespace Secyud.Abp.Secits.AspNetCore.Components.Bundling;

public class ScriptBundler(
    IMinifier minifier,
    IOptions<AbpBundlingOptions> bundlingOptions,
    IBundleFileProvider bundleFileProvider)
    : BundlerBase(minifier, bundlingOptions), IScriptBundler
{
    protected override IFileInfo FindFileInfo(string file)
    {
        return bundleFileProvider.GetFileInfo(file);
    }

    public override string FileExtension => "js";
}