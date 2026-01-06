using Microsoft.AspNetCore.Components;

namespace Secyud.Abp.Secits.AspNetCore.Components.Toolbars;

public partial class MainToolbar
{
    [Inject]
    private IToolbarManager ToolbarManager { get; set; } = null!;

    public List<ToolbarItem> ToolbarItems { get; } = [];
    public List<ToolbarItem> FixedToolbarItems { get; } = [];

    protected override async Task OnInitializedAsync()
    {
        AddToolBar(await ToolbarManager.GetAsync(StandardToolbars.Main));
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