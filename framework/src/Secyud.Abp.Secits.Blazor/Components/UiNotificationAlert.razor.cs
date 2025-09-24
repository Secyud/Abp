using System.Timers;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Secyud.Secits.Blazor;
using Secyud.Secits.Blazor.Icons;
using Volo.Abp.AspNetCore.Components.Notifications;
using Volo.Abp.Localization;
using Timer = System.Timers.Timer;

namespace Secyud.Abp.Secits.Blazor.Components;

public partial class UiNotificationAlert : ComponentBase, IDisposable
{
    private int _index = 0;

    [Inject]
    private ILogger<UiNotificationAlert> Logger { get; set; } = null!;

    [Inject]
    protected IStringLocalizer<AbpUiResource> L { get; set; } = null!;

    [Inject]
    protected SecitsUiNotificationService? UiNotificationService { get; set; }

    [Inject]
    protected IStringLocalizerFactory StringLocalizerFactory { get; set; } = null!;

    [Inject]
    protected IIconProvider IconProvider { get; set; } = null!;

    [Parameter]
    public EventCallback Ensured { get; set; }

    [Parameter]
    public EventCallback Canceled { get; set; }

    protected List<UiNotification> Notifications { get; } = [];

    protected virtual Theme GetSnackbarColor(UiNotificationType notificationType)
    {
        return notificationType switch
        {
            UiNotificationType.Info => Theme.Info,
            UiNotificationType.Success => Theme.Success,
            UiNotificationType.Warning => Theme.Warning,
            UiNotificationType.Error => Theme.Danger,
            _ => Theme.Default,
        };
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (UiNotificationService != null)
        {
            UiNotificationService.NotificationReceived += OnNotificationReceived;
        }
    }

    protected virtual async void OnNotificationReceived(object? sender, UiNotificationEventArgs args)
    {
        try
        {
            var notification = new UiNotification(args.Message, args.Options, _index++ % 65536)
            {
                Title = args.Title,
                Type = args.NotificationType,
                Theme = GetSnackbarColor(args.NotificationType),
                ButtonText = args.Options.OkButtonText == null
                    ? L["OK"]
                    : await args.Options.OkButtonText.LocalizeAsync(StringLocalizerFactory)
            };

            Notifications.Add(notification);
            
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while showing notification.");
        }
    }

    protected async Task CloseNotificationAsync(UiNotification notify)
    {
        Notifications.Remove(notify);
        await InvokeAsync(StateHasChanged);
    }

    protected async Task EnsureNotificationAsync(UiNotification notify)
    {
        await CloseNotificationAsync(notify);
        await Canceled.InvokeAsync(notify);
    }

    protected async Task CancelNotificationAsync(UiNotification notify)
    {
        await CloseNotificationAsync(notify);
        await Ensured.InvokeAsync(notify);
    }

    protected RenderFragment GenerateNotifications() =>
        builder =>
        {
            foreach (var notify in Notifications)
            {
                builder.AddContent(notify.Index, GenerateNotificationCard(notify));
            }
        };

    public virtual void Dispose()
    {
        if (UiNotificationService != null)
        {
            UiNotificationService.NotificationReceived -= OnNotificationReceived;
        }
    }
}