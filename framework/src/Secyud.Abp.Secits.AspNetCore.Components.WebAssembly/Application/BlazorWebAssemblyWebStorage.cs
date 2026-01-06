using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Secits.AspNetCore.Components.Application;

[Dependency(ReplaceServices = true)]
public class BlazorWebAssemblyWebStorage(
    ILocalStorageService localStorageService
) : IWebStorage,ITransientDependency
{
    public async Task<string?> GetValueAsync(string name)
    {
        return await localStorageService.GetItemAsync(name);
    }

    public async Task SetValueAsync(string name, string? value)
    {
        if (value is null)
        {
            await localStorageService.RemoveItemAsync(name);
        }
        else
        {
            await localStorageService.SetItemAsync(name, value);
        }
    }
}