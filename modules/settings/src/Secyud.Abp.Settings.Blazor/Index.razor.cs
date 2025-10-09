using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Secyud.Abp.Settings.Localization;
using Secyud.Abp.Settings.Menus;
using Secyud.Secits.Blazor;
using Volo.Abp.Features;

namespace Secyud.Abp.Settings;

[Authorize]
[RequiresFeature(SettingsFeatures.Enable)]
[Route(SettingsMenus.DefaultUrl)]
public partial class Index
{
    [Inject]
    protected IServiceProvider ServiceProvider { get; set; } = null!;

    [Inject]
    protected IOptions<SettingsComponentOptions> ComponentOptions { get; set; } = null!;

    [Inject]
    protected IStringLocalizer<AbpSettingsResource> L { get; set; } = null!;

    protected SettingsComponentOptions Options => ComponentOptions.Value;

    protected SettingComponentCreationContext SettingComponentCreationContext { get; set; } = null!;
    protected List<RenderFragment> SettingItemRenders { get; set; } = [];

    protected STabContainer? Tab { get; set; }


    protected override async Task OnInitializedAsync()
    {
        SettingComponentCreationContext = new SettingComponentCreationContext(ServiceProvider);

        foreach (var contributor in Options.Contributors)
        {
            await contributor.ConfigureAsync(SettingComponentCreationContext);
        }

        SettingComponentCreationContext.Normalize();
        SettingItemRenders.Clear();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Task.Yield();
            await InvokeAsync(StateHasChanged);
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}