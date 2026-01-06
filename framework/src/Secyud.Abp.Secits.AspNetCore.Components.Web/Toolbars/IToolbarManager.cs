namespace Secyud.Abp.Secits.AspNetCore.Components.Toolbars;

public interface IToolbarManager
{
    Task<Toolbar> GetAsync(string name);
}
