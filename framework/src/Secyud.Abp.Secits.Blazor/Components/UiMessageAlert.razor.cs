using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Secyud.Secits.Blazor.Element;
using Secyud.Secits.Blazor.Icons;
using Volo.Abp.AspNetCore.Components.Messages;

namespace Secyud.Abp.Secits.Blazor.Components;

public partial class UiMessageAlert : IDisposable
{
    protected string? InfoIcon { get; set; }
    protected string? SuccessIcon { get; set; }
    protected string? WarningIcon { get; set; }
    protected string? ErrorIcon { get; set; }
    protected string? ConfirmationIcon { get; set; }
    protected SPopup PopupRef { get; set; } = null!;

    [Inject]
    private ILogger<UiMessageAlert> Logger { get; set; } = null!;

    [Inject]
    private IIconProvider IconProvider { get; set; } = null!;

    [Inject]
    protected SecitsUiMessageService? UiMessageService { get; set; }

    protected virtual bool IsConfirmation => MessageType == UiMessageType.Confirmation;

    protected virtual bool CenterMessage => Options?.CenterMessage ?? true;

    protected virtual bool ShowMessageIcon => Options?.ShowMessageIcon ?? true;

    protected virtual object? MessageIcon =>
        Options?.MessageIcon ?? MessageType switch
        {
            UiMessageType.Info => InfoIcon,
            UiMessageType.Success => SuccessIcon,
            UiMessageType.Warning => WarningIcon,
            UiMessageType.Error => ErrorIcon,
            UiMessageType.Confirmation => ConfirmationIcon,
            _ => null,
        };

    protected virtual string? MessageIconColor =>
        MessageType switch
        {
            // gets the color in the order of importance: Blazorise > Bootstrap > fallback color
            UiMessageType.Info => "var(--b-theme-info, var(--info, #17a2b8))",
            UiMessageType.Success => "var(--b-theme-success, var(--success, #28a745))",
            UiMessageType.Warning => "var(--b-theme-warning, var(--warning, #ffc107))",
            UiMessageType.Error => "var(--b-theme-danger, var(--danger, #dc3545))",
            UiMessageType.Confirmation => "var(--b-theme-secondary, var(--secondary, #6c757d))",
            _ => null,
        };

    protected virtual string MessageIconStyle
    {
        get
        {
            var sb = new StringBuilder();

            sb.Append($"color:{MessageIconColor}");

            return sb.ToString();
        }
    }

    protected virtual string OkButtonText => Options?.OkButtonText ?? "OK";

    protected virtual string ConfirmButtonText => Options?.ConfirmButtonText ?? "Confirm";

    protected virtual string CancelButtonText => Options?.CancelButtonText ?? "Cancel";

    [Parameter]
    public UiMessageType MessageType { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string Message { get; set; } = string.Empty;

    [Parameter]
    public TaskCompletionSource<bool>? Callback { get; set; }

    [Parameter]
    public UiMessageOptions? Options { get; set; }

    [Parameter]
    public EventCallback Okayed { get; set; }

    [Parameter]
    public EventCallback Confirmed { get; set; }

    [Parameter]
    public EventCallback Canceled { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (UiMessageService != null)
        {
            UiMessageService.MessageReceived += OnMessageReceived;
        }

        InfoIcon = IconProvider.GetIcon(IconName.Exclamation);
        SuccessIcon = IconProvider.GetIcon(IconName.Check);
        WarningIcon = IconProvider.GetIcon(IconName.Exclamation);
        ErrorIcon = IconProvider.GetIcon(IconName.Cross);
        ConfirmationIcon = IconProvider.GetIcon(IconName.Question);
    }

    private async void OnMessageReceived(object? sender, UiMessageEventArgs args)
    {
        try
        {
            MessageType = args.MessageType;
            Message = args.Message;
            Title = args.Title;
            Options = args.Options;
            Callback = args.Callback;

            await ShowMessageAlert();
        }
        catch (Exception e)
        {
            Logger.LogException(e);
        }
    }

    protected virtual async Task ShowMessageAlert()
    {
        await InvokeAsync(PopupRef.ShowAsync);
    }

    public void Dispose()
    {
        if (UiMessageService != null)
        {
            UiMessageService.MessageReceived -= OnMessageReceived;
        }
    }

    protected async Task OnOkClicked()
    {
        await InvokeAsync(async () =>
        {
            await Okayed.InvokeAsync(null);

            await PopupRef.HideAsync();
        });
    }

    protected async Task OnConfirmClicked()
    {
        await InvokeAsync(async () =>
        {
            await PopupRef.HideAsync();

            if (IsConfirmation && Callback != null)
            {
                await InvokeAsync(() => Callback.SetResult(true));
            }

            await Confirmed.InvokeAsync(null);
        });
    }

    protected async Task OnCancelClicked()
    {
        await InvokeAsync(async () =>
        {
            await PopupRef.HideAsync();

            if (IsConfirmation && Callback != null)
            {
                await InvokeAsync(() => Callback.SetResult(false));
            }

            await Canceled.InvokeAsync(null);
        });
    }
}