namespace Secyud.Abp.Features;

public interface IMethodInvocationFeatureCheckerService
{
    Task CheckAsync(
        MethodInvocationFeatureCheckerContext context
    );
}
