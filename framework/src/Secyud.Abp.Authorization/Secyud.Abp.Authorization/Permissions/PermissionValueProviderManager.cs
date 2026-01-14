using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization.Permissions;

public class PermissionValueProviderManager : IPermissionValueProviderManager, ISingletonDependency
{
    public IReadOnlyList<IPermissionValueProvider> ValueProviders => _lazyProviders.Value;
    private readonly Lazy<List<IPermissionValueProvider>> _lazyProviders;

    protected AbpPermissionOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }

    public PermissionValueProviderManager(
        IServiceProvider serviceProvider,
        IOptions<AbpPermissionOptions> options)
    {
        Options = options.Value;
        ServiceProvider = serviceProvider;

        _lazyProviders = new Lazy<List<IPermissionValueProvider>>(GetProviders, true);
    }

    protected virtual List<IPermissionValueProvider> GetProviders()
    {
        var providers = Options
            .ValueProviders
            .Select(type => (ServiceProvider.GetRequiredService(type) as IPermissionValueProvider)!)
            .ToList();

        var multipleProviders = providers.GroupBy(p => p.Name).FirstOrDefault(x => x.Count() > 1);
        return multipleProviders != null
            ? throw new AbpException(
                $"Duplicate permission value provider name detected: {multipleProviders.Key}. " +
                $"Providers:{Environment.NewLine}{multipleProviders.Select(p => p.GetType().FullName!).JoinAsString(Environment.NewLine)}")
            : providers;
    }
}