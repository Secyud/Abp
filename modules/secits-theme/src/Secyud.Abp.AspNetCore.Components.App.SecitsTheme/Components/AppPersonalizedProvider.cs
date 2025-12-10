using System.Collections.Concurrent;
using System.Text.Json;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.AspNetCore.Components;

[Dependency(ReplaceServices = true)]
public class AppPersonalizedProvider : IPersonalizedProvider, ISingletonDependency
{
    public AppPersonalizedProvider()
    {
        try
        {
            if (!File.Exists("personalized.json")) return;
            var str = File.ReadAllText("personalized.json");
            _parameters = JsonSerializer.Deserialize<ConcurrentDictionary<string, string>>(str) ?? [];
        }
        catch
        {
            // ignored
        }
    }

    private readonly ConcurrentDictionary<string, string> _parameters = [];

    public Task<string?> GetValueAsync(string name)
    {
        return Task.FromResult(_parameters.GetValueOrDefault(name));
    }

    public Task SetValueAsync(string name, string? value)
    {
        if (value is null)
            _parameters.TryRemove(name, out _);
        else
            _parameters[name] = value;
        return Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        try
        {
            var str = JsonSerializer.Serialize(_parameters);
            await File.WriteAllTextAsync("personalized.json", str);
        }
        catch
        {
            // ignored
        }
    }
}