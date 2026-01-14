using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization.Permissions;

[Dependency(TryRegister = true)]
public class NullPermissionStore : IPermissionStore, ITransientDependency
{
    public ILogger<NullPermissionStore> Logger { get; set; } = NullLogger<NullPermissionStore>.Instance;

    public Task<PermissionGrantResult> IsGrantedAsync(string name, string providerName, string providerKey)
    {
        return Task.FromResult(PermissionGrantResult.Granted);
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
    {
        return Task.FromResult(new MultiplePermissionGrantResult(names, PermissionGrantResult.Granted));
    }
}