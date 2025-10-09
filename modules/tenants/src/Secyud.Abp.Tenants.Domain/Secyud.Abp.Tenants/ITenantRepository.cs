using Volo.Abp.Domain.Repositories;

namespace Secyud.Abp.Tenants;

public interface ITenantRepository : IBasicRepository<Tenant, Guid>
{
    Task<Tenant?> FindByNameAsync(
        string normalizedName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<List<Tenant>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string? filter = null,
        CancellationToken cancellationToken = default);
}
