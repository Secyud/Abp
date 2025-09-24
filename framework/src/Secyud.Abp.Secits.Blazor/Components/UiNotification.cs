using Secyud.Secits.Blazor;
using Volo.Abp.AspNetCore.Components.Notifications;

namespace Secyud.Abp.Secits.Blazor.Components;
using Timer = System.Timers.Timer;

public class UiNotification
{
    public UiNotification(string message, UiNotificationOptions options, int index)
    {
        Message = message;
        Options = options;
        Index = index;
    }

    public int Index { get;  }
    public string? Title { get; set; }
    public UiNotificationType Type { get; set; }
    public Theme Theme { get; set; }
    public string Message { get; set; }
    
    public Timer? Timer { get; set; }
    
    public string? ButtonText { get; set; }
    
    public UiNotificationOptions Options { get; set;}
}