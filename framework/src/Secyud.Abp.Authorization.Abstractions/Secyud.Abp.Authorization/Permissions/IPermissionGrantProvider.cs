namespace Secyud.Abp.Authorization.Permissions;

public interface IPermissionGrantProvider
{
    string Name { get; }

    Task<PermissionGrantResult> CheckAsync(PermissionGrantCheckContext context);

    Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context);
}