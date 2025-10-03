using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Features;

[RemoteService(Name = FeaturesRemoteServiceConsts.RemoteServiceName)]
[Area(FeaturesRemoteServiceConsts.ModuleName)]
[Route("api/features/feature")]
public class FeaturesController(IFeatureAppService featureAppService) : AbpControllerBase, IFeatureAppService
{
    protected IFeatureAppService FeatureAppService { get; } = featureAppService;

    [HttpGet]
    public virtual Task<GetFeatureListResultDto> GetAsync(string? providerName, string? providerKey)
    {
        return FeatureAppService.GetAsync(providerName, providerKey);
    }

    [HttpPut]
    public virtual Task UpdateAsync(string? providerName, string? providerKey, UpdateFeaturesDto input)
    {
        return FeatureAppService.UpdateAsync(providerName, providerKey, input);
    }

    [HttpDelete]
    public virtual Task DeleteAsync(string? providerName, string? providerKey)
    {
        return FeatureAppService.DeleteAsync(providerName, providerKey);
    }
}