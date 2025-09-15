namespace Secyud.Abp.AspNetCore.Toolbars;

public interface IToolbarManager
{
    Task<Toolbar> GetAsync(string name);
}
