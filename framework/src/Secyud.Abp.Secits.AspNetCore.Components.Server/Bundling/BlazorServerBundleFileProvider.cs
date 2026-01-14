using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Secits.AspNetCore.Components.Bundling;

public class BlazorServerBundleFileProvider : IBundleFileProvider, ITransientDependency
{
    protected IWebHostEnvironment WebHostingEnvironment { get; }

    protected BlazorServerBundleFileProvider(
        IWebHostEnvironment webHostingEnvironment)
    {
        WebHostingEnvironment = webHostingEnvironment;
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return WebHostingEnvironment.WebRootFileProvider.GetDirectoryContents(subpath);
    }

    public IFileInfo GetFileInfo(string file)
    {
        return WebHostingEnvironment.WebRootFileProvider.GetFileInfo(file);
    }

    public IChangeToken Watch(string filter)
    {
        return WebHostingEnvironment.WebRootFileProvider.Watch(filter);
    }
}