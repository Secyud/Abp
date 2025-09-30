using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Volo.Abp.DependencyInjection;

namespace SecitsDemoApp.HealthChecks;

public class SecitsDemoAppDatabaseCheck : IHealthCheck, ITransientDependency
{
    // protected readonly IIdentityRoleRepository IdentityRoleRepository;
    //
    // public SecitsDemoAppDatabaseCheck(IIdentityRoleRepository identityRoleRepository)
    // {
    //     IdentityRoleRepository = identityRoleRepository;
    // }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // await IdentityRoleRepository.GetListAsync(sorting: nameof(IdentityRole.Id), maxResultCount: 1, cancellationToken: cancellationToken);
            await Task.CompletedTask;
            return HealthCheckResult.Healthy($"Could connect to database and get record.");
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy($"Error when trying to get database record. ", e);
        }
    }
}