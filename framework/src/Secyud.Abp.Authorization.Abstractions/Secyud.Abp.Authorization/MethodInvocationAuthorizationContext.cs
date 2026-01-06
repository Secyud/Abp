using System.Reflection;

namespace Secyud.Abp.Authorization;

public class MethodInvocationAuthorizationContext(MethodInfo method)
{
    public MethodInfo Method { get; } = method;
}
