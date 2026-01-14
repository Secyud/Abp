using System.Reflection;

namespace Secyud.Abp.Features;

public class MethodInvocationFeatureCheckerContext(MethodInfo method)
{
    public MethodInfo Method { get; } = method;
}
