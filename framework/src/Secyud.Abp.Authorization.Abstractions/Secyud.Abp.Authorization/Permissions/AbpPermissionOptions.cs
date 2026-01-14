using Volo.Abp.Collections;

namespace Secyud.Abp.Authorization.Permissions;

public class AbpPermissionOptions
{
    public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; } =
        new TypeList<IPermissionDefinitionProvider>();

    public ITypeList<IPermissionValueProvider> ValueProviders { get; } =
        new TypeList<IPermissionValueProvider>();
}