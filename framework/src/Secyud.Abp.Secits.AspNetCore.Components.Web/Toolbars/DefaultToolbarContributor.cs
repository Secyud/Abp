namespace Secyud.Abp.Secits.AspNetCore.Components.Toolbars;

public class DefaultToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(GeneralSettings), fix: true));
        }

        return Task.CompletedTask;
    }
}