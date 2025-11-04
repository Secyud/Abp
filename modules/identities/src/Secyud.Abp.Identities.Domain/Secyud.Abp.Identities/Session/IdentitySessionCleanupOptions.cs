using Volo.Abp.BackgroundWorkers;

namespace Secyud.Abp.Identities.Session;

public class IdentitySessionCleanupOptions
{
    /// <summary>
    /// Default value: true.
    /// If <see cref="AbpBackgroundWorkerOptions.IsEnabled"/> is false,
    /// this property is ignored and the cleanup worker doesn't work for this application instance.
    /// </summary>
    public bool IsCleanupEnabled { get; set; } = true;

    /// <summary>
    /// Default: 3,600,000 ms.
    /// </summary>
    public int CleanupPeriod { get; set; } = 3_600_000;

    /// <summary>
    /// The sessions that have not been active in the past will be deleted.
    /// Default: 30 days.
    /// </summary>
    public TimeSpan InactiveTimeSpan { get; set; } = TimeSpan.FromDays(30);
}
