using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Features;

namespace Secyud.Abp.Features;

[Authorize]
public class FeatureAppService(
    IFeatureManager featureManager,
    IFeatureDefinitionManager featureDefinitionManager,
    IOptions<FeaturesOptions> options)
    : FeaturesAppServiceBase, IFeatureAppService
{
    protected FeaturesOptions Options { get; } = options.Value;
    protected IFeatureManager FeatureManager { get; } = featureManager;
    protected IFeatureDefinitionManager FeatureDefinitionManager { get; } = featureDefinitionManager;

    public virtual async Task<GetFeatureListResultDto> GetAsync(string? providerName, string? providerKey)
    {
        await CheckProviderPolicy(providerName, providerKey);

        var result = new GetFeatureListResultDto
        {
            Groups = new List<FeatureGroupDto>()
        };

        foreach (var group in await FeatureDefinitionManager.GetGroupsAsync())
        {
            var groupDto = CreateFeatureGroupDto(group);

            foreach (var featureDefinition in group.GetFeaturesWithChildren())
            {
                if (providerName == TenantFeatureValueProvider.ProviderName &&
                    CurrentTenant.Id == null &&
                    providerKey == null &&
                    !featureDefinition.IsAvailableToHost)
                {
                    continue;
                }

                var feature = await FeatureManager.GetOrNullWithProviderAsync(featureDefinition.Name, providerName, providerKey);
                groupDto.Features.Add(CreateFeatureDto(feature, featureDefinition));
            }

            SetFeatureDepth(groupDto.Features, providerName, providerKey);

            if (groupDto.Features.Any())
            {
                result.Groups.Add(groupDto);
            }
        }

        return result;
    }

    private FeatureGroupDto CreateFeatureGroupDto(FeatureGroupDefinition groupDefinition)
    {
        return new FeatureGroupDto
        {
            Name = groupDefinition.Name,
            DisplayName = groupDefinition.DisplayName.Localize(StringLocalizerFactory),
            Features = new List<FeatureDto>()
        };
    }

    private FeatureDto CreateFeatureDto(FeatureNameValueWithGrantedProvider featureNameValueWithGrantedProvider, FeatureDefinition featureDefinition)
    {
        return new FeatureDto
        {
            Name = featureDefinition.Name,
            DisplayName = featureDefinition.DisplayName.Localize(StringLocalizerFactory),
            Description = featureDefinition.Description?.Localize(StringLocalizerFactory)!,

            ValueType = featureDefinition.ValueType,

            ParentName = featureDefinition.Parent?.Name,
            Value = featureNameValueWithGrantedProvider.Value,
            Provider = new FeatureProviderDto
            {
                Name = featureNameValueWithGrantedProvider.Provider?.Name,
                Key = featureNameValueWithGrantedProvider.Provider?.Key
            }
        };
    }

    public virtual async Task UpdateAsync(string? providerName, string? providerKey, UpdateFeaturesDto input)
    {
        await CheckProviderPolicy(providerName, providerKey);

        foreach (var feature in input.Features)
        {
            await FeatureManager.SetAsync(feature.Name, feature.Value, providerName, providerKey);
        }
    }

    protected virtual void SetFeatureDepth(List<FeatureDto> features, string? providerName, string? providerKey,
        FeatureDto? parentFeature = null, int depth = 0)
    {
        foreach (var feature in features)
        {
            if ((parentFeature == null && feature.ParentName == null) || (parentFeature != null && parentFeature.Name == feature.ParentName))
            {
                feature.Depth = depth;
                SetFeatureDepth(features, providerName, providerKey, feature, depth + 1);
            }
        }
    }

    protected virtual async Task CheckProviderPolicy(string? providerName, string? providerKey)
    {
        string? policyName;
        if (providerName == TenantFeatureValueProvider.ProviderName && CurrentTenant.Id == null && providerKey == null)
        {
            policyName = FeaturesPermissions.ManageHostFeatures;
        }
        else
        {
            policyName = providerName is null ? null : Options.ProviderPolicies.GetOrDefault(providerName);
            if (policyName.IsNullOrEmpty())
            {
                throw new AbpException(
                    $"No policy defined to get/set permissions for the provider '{providerName}'. Use {nameof(FeaturesOptions)} to map the policy.");
            }
        }

        await AuthorizationService.CheckAsync(policyName);
    }

    public virtual async Task DeleteAsync(string? providerName, string? providerKey)
    {
        await FeatureManager.DeleteAsync(providerName, providerKey);
    }
}