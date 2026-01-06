using Secyud.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Permissions;

public class TestPermissionDefinitionProvider : IPermissionDefinitionProvider
{
    public void PreDefine(PermissionDefinitionContext context)
    {
    }

    public void Define(PermissionDefinitionContext context)
    {
        var testGroup = context.AddGroup("TestGroup");

        testGroup.AddPermission("MyPermission1");
        testGroup.AddPermission("MyDisabledPermission1", isEnabled: false);

        var myPermission2 = testGroup.AddPermission("MyPermission2");
        myPermission2.AddChild("MyPermission2.ChildPermission1");

        testGroup.AddPermission("MyPermission3", multiTenancySide: MultiTenancySides.Host);

        testGroup.AddPermission("MyPermission4", multiTenancySide: MultiTenancySides.Host)
            .WithProviders(UserPermissionValueProvider.ProviderName);

        var myPermission5 = testGroup.AddPermission("MyPermission5");
        myPermission5.StateCheckers.Add(new TestRequireRolePermissionStateProvider("super-admin"));
        myPermission5.AddChild("MyPermission5.ChildPermission1");

        var myPermission6 = testGroup.AddPermission("MyPermission6");
        myPermission6.AddChild("MyPermission6.ChildDisabledPermission1", isEnabled: false);
        myPermission6.AddChild("MyPermission6.ChildPermission2");

        var myDisabledPermission2 = testGroup.AddPermission("MyDisabledPermission2", isEnabled: false);
        myDisabledPermission2.AddChild("MyDisabledPermission2.ChildPermission1");
        var myDisabledPermission2Child2 = myDisabledPermission2.AddChild("MyDisabledPermission2.ChildPermission2");
        myDisabledPermission2Child2.AddChild("MyDisabledPermission2.ChildPermission2.ChildPermission1");

        var testGroup2 = context.AddGroup("TestGroup2");
        testGroup2.AddPermission("MyPermission7");
        testGroup2.AddPermission("MyPermission8");
    }

    public void PostDefine(PermissionDefinitionContext context)
    {
    }
}