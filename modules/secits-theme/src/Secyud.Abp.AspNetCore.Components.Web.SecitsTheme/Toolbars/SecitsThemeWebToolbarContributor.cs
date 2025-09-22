using Secyud.Abp.AspNetCore.Components.Toolbars;

namespace Secyud.Abp.AspNetCore.Toolbars;

public class SecitsThemeWebToolbarContributor : IToolbarContributor
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