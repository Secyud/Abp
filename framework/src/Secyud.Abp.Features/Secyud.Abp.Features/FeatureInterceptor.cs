using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Secyud.Abp.Features;

public class FeatureInterceptor(IServiceScopeFactory serviceScopeFactory) : AbpInterceptor, ITransientDependency
{
    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.FeatureChecking))
        {
            await invocation.ProceedAsync();
            return;
        }

        await CheckFeaturesAsync(invocation);
        await invocation.ProceedAsync();
    }

    protected virtual async Task CheckFeaturesAsync(IAbpMethodInvocation invocation)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IMethodInvocationFeatureCheckerService>().CheckAsync(
            new MethodInvocationFeatureCheckerContext(
                invocation.Method
            )
        );
    }
}
