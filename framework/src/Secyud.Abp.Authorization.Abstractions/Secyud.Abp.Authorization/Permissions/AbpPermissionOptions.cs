using Volo.Abp.Collections;

namespace Secyud.Abp.Authorization.Permissions;

public class AbpPermissionOptions
{
    public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; } =
        new TypeList<IPermissionDefinitionProvider>();

    public ITypeList<IPermissionGrantProvider> GrantProviders { get; } =
        new TypeList<IPermissionGrantProvider>();
}