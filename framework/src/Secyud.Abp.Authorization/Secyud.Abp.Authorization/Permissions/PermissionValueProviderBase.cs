using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization.Permissions;

public abstract class PermissionValueProviderBase(IPermissionStore permissionStore)
    : IPermissionValueProvider, ITransientDependency
{
    public abstract string Name { get; }
    protected IPermissionStore PermissionStore { get; } = permissionStore;
    public abstract Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context);

    public abstract Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context);
}