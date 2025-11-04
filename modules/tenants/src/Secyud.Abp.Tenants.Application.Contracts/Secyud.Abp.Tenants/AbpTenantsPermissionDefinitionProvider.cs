using Volo.Abp.Authorization.Permissions;

namespace Secyud.Abp.Tenants;

public class AbpTenantsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var tenantsGroup = context.AddGroup(TenantsPermissions.Group);

        var tenantsPermission = tenantsGroup.AddPermission(TenantsPermissions.Tenants.Default);
        tenantsPermission.AddChild(TenantsPermissions.Tenants.Create);
        tenantsPermission.AddChild(TenantsPermissions.Tenants.Update);
        tenantsPermission.AddChild(TenantsPermissions.Tenants.Delete);
        tenantsPermission.AddChild(TenantsPermissions.Tenants.ManageFeatures);
        tenantsPermission.AddChild(TenantsPermissions.Tenants.ManageConnectionStrings);
    }
}