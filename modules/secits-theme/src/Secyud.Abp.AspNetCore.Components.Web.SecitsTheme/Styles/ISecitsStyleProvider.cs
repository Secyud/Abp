namespace Secyud.Abp.AspNetCore.Styles;

public interface ISecitsStyleProvider
{
    Task<string> GetCurrentStyleAsync();
}