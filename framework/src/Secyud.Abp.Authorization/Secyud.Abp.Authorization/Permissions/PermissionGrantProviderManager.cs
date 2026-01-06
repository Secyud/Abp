using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionGrantProviderManager : IPermissionGrantProviderManager, ISingletonDependency
{
    public IReadOnlyList<IPermissionGrantProvider> ValueProviders => _lazyProviders.Value;
    private readonly Lazy<List<IPermissionGrantProvider>> _lazyProviders;

    protected AbpPermissionOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }

    public PermissionGrantProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpPermissionOptions> options)
    {
        Options = options.Value;
        ServiceProvider = serviceProvider;

        _lazyProviders = new Lazy<List<IPermissionGrantProvider>>(GetProviders, true);
    }

    protected virtual List<IPermissionGrantProvider> GetProviders()
    {
        var providers = Options
            .GrantProviders
            .Select(type => (ServiceProvider.GetRequiredService(type) as IPermissionGrantProvider)!)
            .ToList();

        var multipleProviders = providers.GroupBy(p => p.Name).FirstOrDefault(x => x.Count() > 1);
        return multipleProviders != null
            ? throw new AbpException(
                $"Duplicate permission value provider name detected: {multipleProviders.Key}. " +
                $"Providers:{Environment.NewLine}{multipleProviders.Select(p => p.GetType().FullName!).JoinAsString(Environment.NewLine)}")
            : providers;
    }
}