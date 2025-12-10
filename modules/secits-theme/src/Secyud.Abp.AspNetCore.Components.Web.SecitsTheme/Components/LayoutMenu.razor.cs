using Microsoft.AspNetCore.Components;

namespace Secyud.Abp.AspNetCore.Components;

public partial class LayoutMenu
{
    [Inject] protected IPersonalizedProvider PersonalizedProvider { get; set; } = null!;

    protected bool MenuFixed { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? Name { get; set; }

    protected string MenuFixedKey => $"LayoutMenu.{Name}.MenuFixed";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var isFixed = await PersonalizedProvider.GetValueAsync(MenuFixedKey);
            MenuFixed = bool.TryParse(isFixed, out var result) && result;
            await InvokeAsync(StateHasChanged);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected string GetClass()
    {
        List<string> classes = [];
        if (Class is not null)
            classes.Add(Class);
        if (MenuFixed)
            classes.Add("fixed");

        return string.Join(' ', classes);
    }

    protected async Task ChangeFixedAsync()
    {
        MenuFixed = !MenuFixed;
        await PersonalizedProvider.SetValueAsync(MenuFixedKey, MenuFixed.ToString());
        await InvokeAsync(StateHasChanged);
    }
}