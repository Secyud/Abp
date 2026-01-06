using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Secyud.Abp.Authorization;

public class AuthorizationInterceptor(IMethodInvocationAuthorizationService methodInvocationAuthorizationService)
    : AbpInterceptor, ITransientDependency
{
    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        await AuthorizeAsync(invocation);
        await invocation.ProceedAsync();
    }

    protected virtual async Task AuthorizeAsync(IAbpMethodInvocation invocation)
    {
        await methodInvocationAuthorizationService.CheckAsync(
            new MethodInvocationAuthorizationContext(
                invocation.Method
            )
        );
    }
}
