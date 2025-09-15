using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Secyud.Secits.Blazor;
using Volo.Abp.AspNetCore.Components.Progression;

namespace Secyud.Abp.Secits.Blazor.Components;

public partial class UiPageProgress : IDisposable
{
#if DEBUG
    [Inject]
    private ILogger<UiPageProgress> Logger { get; set; } = null!;
#endif

    protected int Percentage { get; set; }

    protected bool Visible { get; set; }

    protected Theme Color { get; set; }

    [Inject]
    protected IUiPageProgressService? UiPageProgressService { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (UiPageProgressService != null)
        {
            UiPageProgressService.ProgressChanged += OnProgressChanged;
        }
    }

    private async void OnProgressChanged(object? sender, UiPageProgressEventArgs args)
    {
        try
        {
            Percentage = args.Percentage ?? -1;
            Visible = args.Percentage is null or >= 0 and <= 100;
            Color = GetColor(args.Options.Type);

            await InvokeAsync(StateHasChanged);
        }
        catch (Exception e)
        {
#if DEBUG
            Logger.LogException(e);
#endif
        }
    }

    public virtual void Dispose()
    {
        if (UiPageProgressService != null)
        {
            UiPageProgressService.ProgressChanged -= OnProgressChanged;
        }
    }

    protected virtual Theme GetColor(UiPageProgressType pageProgressType)
    {
        return pageProgressType switch
        {
            UiPageProgressType.Info => Theme.Info,
            UiPageProgressType.Success => Theme.Success,
            UiPageProgressType.Warning => Theme.Warning,
            UiPageProgressType.Error => Theme.Danger,
            _ => Theme.Default
        };
    }
}