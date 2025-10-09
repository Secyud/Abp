using Volo.Abp.Application.Services;

namespace Secyud.Abp.Features;

public interface IFeatureAppService : IApplicationService
{
    Task<GetFeatureListResultDto> GetAsync(string? providerName, string? providerKey);

    Task UpdateAsync(string? providerName, string? providerKey, UpdateFeaturesDto input);

    Task DeleteAsync(string? providerName, string? providerKey);
}