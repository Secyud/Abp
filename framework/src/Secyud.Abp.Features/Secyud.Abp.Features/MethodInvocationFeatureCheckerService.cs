using System.Reflection;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Features;

public class MethodInvocationFeatureCheckerService(IFeatureChecker featureChecker)
    : IMethodInvocationFeatureCheckerService, ITransientDependency
{
    public async Task CheckAsync(MethodInvocationFeatureCheckerContext context)
    {
        if (IsFeatureCheckDisabled(context))
        {
            return;
        }

        foreach (var requiresFeatureAttribute in GetRequiredFeatureAttributes(context.Method))
        {
            await featureChecker.CheckEnabledAsync(requiresFeatureAttribute.RequiresAll, requiresFeatureAttribute.Features);
        }
    }

    protected virtual bool IsFeatureCheckDisabled(MethodInvocationFeatureCheckerContext context)
    {
        return context.Method
            .GetCustomAttributes(true)
            .OfType<DisableFeatureCheckAttribute>()
            .Any();
    }

    protected virtual IEnumerable<RequiresFeatureAttribute> GetRequiredFeatureAttributes(MethodInfo methodInfo)
    {
        var attributes = methodInfo
            .GetCustomAttributes(true)
            .OfType<RequiresFeatureAttribute>();

        if (methodInfo.IsPublic)
        {
            attributes = attributes
                .Union(
                    methodInfo.DeclaringType!
                        .GetCustomAttributes(true)
                        .OfType<RequiresFeatureAttribute>()
                );
        }

        return attributes;
    }
}
