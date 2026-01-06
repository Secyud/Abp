using Microsoft.AspNetCore.Components;
using Secyud.Abp.Secits.AspNetCore.Components.Application;

namespace Secyud.Abp.Secits.AspNetCore.Components.Layout;

public partial class MainLayoutMenu
{
    [Inject] protected IWebStorage WebStorage { get; set; } = null!;

    protected bool MenuFixed { get; set; }

    [Parameter] public string? Class { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string? Name { get; set; }

    protected string MenuFixedKey => $"LayoutMenu.{Name}.MenuFixed";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var isFixed = await WebStorage.GetValueAsync(MenuFixedKey);
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
        await WebStorage.SetValueAsync(MenuFixedKey, MenuFixed.ToString());
        await InvokeAsync(StateHasChanged);
    }
}