using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Secits.AspNetCore.Components.Application;

[Dependency(ReplaceServices = true)]
public class BlazorWebViewWebStorage : IWebStorage, ISingletonDependency
{
    protected Dictionary<string, string> Store { get; } = [];

    public Task<string?> GetValueAsync(string name)
    {
        return Task.FromResult(Store.GetValueOrDefault(name));
    }

    public Task SetValueAsync(string name, string? value)
    {
        if (value is null)
        {
            Store.Remove(name);
        }
        else
        {
            Store[name] = value;
        }

        return Task.CompletedTask;
    }
}