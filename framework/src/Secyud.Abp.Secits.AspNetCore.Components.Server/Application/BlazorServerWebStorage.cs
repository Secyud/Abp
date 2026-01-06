using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Secits.AspNetCore.Components.Application;

[Dependency(ReplaceServices = true)]
public class BlazorServerWebStorage(
    IHttpContextAccessor httpContextAccessor
) : IWebStorage,ITransientDependency
{
    public Task<string?> GetValueAsync(string name)
    {
        if (httpContextAccessor.HttpContext is null) return Task.FromResult<string?>(null);

        return Task.FromResult(httpContextAccessor.HttpContext.Request.Cookies
            .TryGetValue(name, out var value)
            ? value
            : null);
    }

    public Task SetValueAsync(string name, string? value)
    {
        if (httpContextAccessor.HttpContext is null) return Task.CompletedTask;

        if (value is null)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(name);
        }
        else
        {
            httpContextAccessor.HttpContext.Response.Cookies.Append(name, value);
        }

        return Task.CompletedTask;
    }
}