namespace Secyud.Abp.Authorization.Permissions;

public interface IPermissionGrantProviderManager
{
    IReadOnlyList<IPermissionGrantProvider> ValueProviders { get; }
}
