namespace Secyud.Abp.Secits.AspNetCore.Components.Toolbars;

public interface IToolbarContributor
{
    Task ConfigureToolbarAsync(IToolbarConfigurationContext context);
}
