namespace Secyud.Abp.Ui.Navigation;

public interface IMenuContributor
{
    Task ConfigureMenuAsync(MenuConfigurationContext context);
}
