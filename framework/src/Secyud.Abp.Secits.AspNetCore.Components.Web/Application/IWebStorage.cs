namespace Secyud.Abp.Secits.AspNetCore.Components.Application;

public interface IWebStorage
{
    Task<string?> GetValueAsync(string name);
    Task SetValueAsync(string name, string? value);
}