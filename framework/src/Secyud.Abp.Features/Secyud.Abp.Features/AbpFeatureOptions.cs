using Volo.Abp.Collections;

namespace Secyud.Abp.Features;

public class AbpFeatureOptions
{
    public ITypeList<IFeatureDefinitionProvider> DefinitionProviders { get; } =
        new TypeList<IFeatureDefinitionProvider>();

    public ITypeList<IFeatureValueProvider> ValueProviders { get; } = new TypeList<IFeatureValueProvider>();
}