using Secyud.Secits.Blazor;
using Volo.Abp.AspNetCore.Components.Notifications;

namespace Secyud.Abp.Secits.Blazor.Components;
using Timer = System.Timers.Timer;

public class UiNotification(string message, UiNotificationOptions options, int index)
{
    public int Index { get;  } = index;
    public string? Title { get; set; }
    public UiNotificationType Type { get; set; }
    public Theme Theme { get; set; }
    public string Message { get; set; } = message;

    public Timer? Timer { get; set; }
    
    public string? ButtonText { get; set; }
    
    public UiNotificationOptions Options { get; set;} = options;
}