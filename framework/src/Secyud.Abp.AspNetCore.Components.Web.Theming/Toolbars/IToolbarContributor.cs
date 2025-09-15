namespace Secyud.Abp.AspNetCore.Toolbars;

public interface IToolbarContributor
{
    Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
}
