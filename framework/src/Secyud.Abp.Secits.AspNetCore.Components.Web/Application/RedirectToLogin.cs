using Microsoft.AspNetCore.Components;

namespace Secyud.Abp.Secits.AspNetCore.Components.Application;

public class RedirectToLogin : ComponentBase
{
    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    protected override void OnInitialized()
    {
        Navigation.NavigateTo($"account/login?returnUrl={Uri.EscapeDataString(Navigation.Uri)}", true);
    }
}