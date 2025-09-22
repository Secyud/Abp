using Microsoft.AspNetCore.Components;
using Secyud.Abp.AspNetCore.Components.Toolbars;

namespace Secyud.Abp.AspNetCore.Toolbars;

public partial class MainToolbar
{
    [Inject]
    private IToolbarManager ToolbarManager { get; set; } = null!;

    public List<ToolbarItem> ToolbarItems { get; } = [];
    public List<ToolbarItem> FixedToolbarItems { get; } = [];

    protected override async Task OnInitializedAsync()
    {
        AddToolBar(await ToolbarManager.GetAsync(StandardToolbars.Main));
        AddToolBar(await ToolbarManager.GetAsync(SecitsToolbars.Main));
    }

    private void AddToolBar(Toolbar toolbar)
    {
        foreach (var item in toolbar.Items)
        {
            var list = item.Fix ? FixedToolbarItems : ToolbarItems;
            var index = 0;
            for (var i = 0; i < list.Count; i++)
            {
                if (item.Order < list[i].Order) continue;
                index = i;
                break;
            }

            list.Insert(index, item);
        }
    }
}