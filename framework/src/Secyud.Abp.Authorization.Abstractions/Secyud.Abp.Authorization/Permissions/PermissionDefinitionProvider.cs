using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Authorization.Permissions;

public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider, ITransientDependency
{
    public void PreDefine(PermissionDefinitionContext context)
    {
    }

    public abstract void Define(PermissionDefinitionContext context);

    public void PostDefine(PermissionDefinitionContext context)
    {
    }
}