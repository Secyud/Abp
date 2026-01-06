namespace Secyud.Abp.Authorization.Permissions;

public interface IPermissionDefinitionProvider
{
    void PreDefine(PermissionDefinitionContext context);

    void Define(PermissionDefinitionContext context);

    void PostDefine(PermissionDefinitionContext context);
}
