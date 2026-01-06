using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.Ui.LayoutHooks;

namespace Secyud.Abp.Secits.AspNetCore.Components.Layout;

public partial class LayoutHook : ComponentBase
{
    [Parameter]
    public string Name { get; set; } = null!;
    
    [Parameter]
    public string? Layout { get; set; }

    [Inject]
    protected IOptions<AbpLayoutHookOptions> LayoutHookOptions { get; set; } = null!;

    protected LayoutHookViewModel LayoutHookViewModel { get; private set; } = null!;

    protected override Task OnInitializedAsync()
    {
        if (LayoutHookOptions.Value.Hooks.TryGetValue(Name, out var layoutHooks))
        {
            layoutHooks = layoutHooks
                .Where(x => IsComponentBase(x) && (string.IsNullOrWhiteSpace(x.Layout) || x.Layout == Layout))
                .ToList();
        }

        layoutHooks ??= [];
        
        LayoutHookViewModel = new LayoutHookViewModel(layoutHooks.ToArray(), Layout);
        
        return Task.CompletedTask;
    }

    protected virtual bool IsComponentBase(LayoutHookInfo layoutHook)
    {
        return typeof(ComponentBase).IsAssignableFrom(layoutHook.ComponentType);
    }
}