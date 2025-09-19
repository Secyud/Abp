namespace Secyud.Abp.AspNetCore.Components.Toolbars;

public interface IToolbarManager
{
    Task<Toolbar> GetAsync(string name);
}
