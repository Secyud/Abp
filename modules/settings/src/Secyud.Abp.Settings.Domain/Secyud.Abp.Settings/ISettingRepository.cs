using Volo.Abp.Domain.Repositories;

namespace Secyud.Abp.Settings;

public interface ISettingRepository : IBasicRepository<Setting, Guid>
{
    Task<Setting?> FindAsync(
        string name,
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default);

    Task<List<Setting>> GetListAsync(
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default);

    Task<List<Setting>> GetListAsync(
        string[] names,
        string? providerName,
        string? providerKey,
        CancellationToken cancellationToken = default);
}