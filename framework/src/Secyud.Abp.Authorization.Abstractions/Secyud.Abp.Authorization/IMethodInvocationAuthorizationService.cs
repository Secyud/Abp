namespace Secyud.Abp.Authorization;

public interface IMethodInvocationAuthorizationService
{
    Task CheckAsync(MethodInvocationAuthorizationContext context);
}
