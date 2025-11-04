using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace Secyud.Abp.Identities.Session;

[BackgroundWorkerName("Secyud.Abp.Identity.Session.IdentitySessionCleanupBackgroundWorker")]
public class IdentitySessionCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    protected IAbpDistributedLock DistributedLock { get; }

    public IdentitySessionCleanupBackgroundWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<IdentitySessionCleanupOptions> cleanupOptions,
        IAbpDistributedLock distributedLock)
        : base(timer, serviceScopeFactory)
    {
        DistributedLock = distributedLock;
        Timer.Period = cleanupOptions.CurrentValue.CleanupPeriod;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await using var handle = await DistributedLock.TryAcquireAsync(nameof(IdentitySessionCleanupBackgroundWorker));
        Logger.LogInformation($"Lock is acquired for {nameof(IdentitySessionCleanupBackgroundWorker)}");

        if (handle != null)
        {
            await workerContext
                .ServiceProvider
                .GetRequiredService<IdentitySessionCleanupService>()
                .CleanAsync();

            Logger.LogInformation($"Lock is released for {nameof(IdentitySessionCleanupBackgroundWorker)}");
            return;
        }

        Logger.LogInformation($"Handle is null because of the locking for : {nameof(IdentitySessionCleanupBackgroundWorker)}");
    }
}
