namespace Secyud.Abp.Authorization.Permissions;

public interface IPermissionStore
{
    Task<PermissionGrantResult> IsGrantedAsync(string name,
        string providerName, string providerKey);

    Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names,
        string providerName, string providerKey);
}