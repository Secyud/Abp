namespace Secyud.Abp.AspNetCore.Components;

public interface IPersonalizedProvider
{
    Task<string?> GetValueAsync(string name);
    Task SetValueAsync(string name, string? value);
}