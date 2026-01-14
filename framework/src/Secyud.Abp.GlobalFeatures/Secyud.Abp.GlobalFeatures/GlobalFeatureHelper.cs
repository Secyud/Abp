using Volo.Abp.Reflection;

namespace Secyud.Abp.GlobalFeatures;

public static class GlobalFeatureHelper
{
    public static bool IsGlobalFeatureEnabled(Type type, out RequiresGlobalFeatureAttribute? attribute)
    {
        attribute = ReflectionHelper.GetSingleAttributeOrDefault<RequiresGlobalFeatureAttribute>(type);
        return attribute == null || GlobalFeatureManager.Instance.IsEnabled(attribute.GetFeatureName());
    }
}
