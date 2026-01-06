using Secyud.Abp.Authorization.Permissions;

namespace Secyud.Abp.Permissions;

public interface IPermissionStore
{
    Task<PermissionGrantResult> IsGrantedAsync(string name,
        string providerName, string providerKey);

    Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names,
        string providerName, string providerKey);
}