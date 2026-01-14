using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;

namespace Secyud.Abp.Secits.AspNetCore.Components.Bundling;

public class BlazorWebViewBundleFileProvider : IBundleFileProvider, ITransientDependency
{
    private readonly IVirtualFileProvider _virtualFileProvider;
    private readonly IFileProvider _fileProvider;
    private const string RootPath = "/wwwroot";

    public BlazorWebViewBundleFileProvider(IVirtualFileProvider virtualFileProvider)
    {
        _virtualFileProvider = virtualFileProvider;
        _fileProvider = CreateFileProvider();
    }

    public string ContentRootPath => Directory.GetCurrentDirectory();

    public IFileInfo GetFileInfo(string subpath)
    {
        if (string.IsNullOrEmpty(subpath))
        {
            return new NotFoundFileInfo(subpath);
        }

        var fileInfo = _fileProvider.GetFileInfo(subpath);
        return fileInfo.Exists ? fileInfo : _fileProvider.GetFileInfo(RootPath + subpath.EnsureStartsWith('/'));
    }


    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        if (string.IsNullOrEmpty(subpath))
        {
            return NotFoundDirectoryContents.Singleton;
        }

        var directory = _fileProvider.GetDirectoryContents(subpath);
        return directory.Exists
            ? directory
            : _fileProvider.GetDirectoryContents(RootPath + subpath.EnsureStartsWith('/'));
    }

    public IChangeToken Watch(string filter)
    {
        return new CompositeChangeToken(
            [
                _fileProvider.Watch(RootPath + filter),
                _fileProvider.Watch(filter)
            ]
        );
    }

    protected IFileProvider CreateFileProvider()
    {
        var assetsDirectory = Path.Combine(ContentRootPath, RootPath.TrimStart('/'));
        if (!Path.Exists(assetsDirectory))
        {
            Directory.CreateDirectory(assetsDirectory);
        }

        return new CompositeFileProvider(new PhysicalFileProvider(assetsDirectory), _virtualFileProvider);
    }
}