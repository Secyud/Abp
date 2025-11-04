using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Identities.Session;

public class IdentitySessionCleanupService(
    IOptionsMonitor<IdentitySessionCleanupOptions> cleanupOptions,
    IdentitySessionManager identitySessionManager)
    : ITransientDependency
{
    public ILogger<IdentitySessionCleanupService> Logger { get; set; } = NullLogger<IdentitySessionCleanupService>.Instance;
    protected IdentitySessionCleanupOptions CleanupOptions { get; } = cleanupOptions.CurrentValue;
    protected IdentitySessionManager IdentitySessionManager { get; } = identitySessionManager;

    public virtual async Task CleanAsync()
    {
        Logger.LogInformation("Start cleanup sessions.");
        try
        {
            await IdentitySessionManager.DeleteAllAsync(CleanupOptions.InactiveTimeSpan);
        }
        catch (Exception exception)
        {
            Logger.LogException(exception);
        }
        finally
        {
            Logger.LogInformation("Cleanup sessions completed.");
        }
    }
}
