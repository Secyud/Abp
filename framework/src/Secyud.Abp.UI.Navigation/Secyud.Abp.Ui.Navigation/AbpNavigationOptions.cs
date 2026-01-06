namespace Secyud.Abp.Ui.Navigation;

public class AbpNavigationOptions
{
    public List<IMenuContributor> MenuContributors { get; } = [];

    public List<string> MainMenuNames { get; } = [];
}