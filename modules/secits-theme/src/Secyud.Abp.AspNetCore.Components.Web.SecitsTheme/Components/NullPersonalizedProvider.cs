using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.AspNetCore.Components;

public class NullPersonalizedProvider : IPersonalizedProvider, ISingletonDependency
{
    public Task<string?> GetValueAsync(string name)
    {
        return Task.FromResult<string?>(null);
    }

    public Task SetValueAsync(string name, string? value)
    {
        return Task.CompletedTask;
    }
}