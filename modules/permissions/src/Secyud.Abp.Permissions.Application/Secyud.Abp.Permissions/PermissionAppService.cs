using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Localization;

namespace Secyud.Abp.Permissions;

[Authorize]
public class PermissionAppServiceBase(
    IPermissionManager manager,
    IPermissionDefinitionRecordRepository recordRepository,
    IPermissionGrantRepository grantRepository,
    ILocalizableStringSerializer serializer)
    : ApplicationService, IPermissionAppService
{
    protected IPermissionDefinitionRecordRepository RecordRepository { get; } = recordRepository;
    protected IPermissionGrantRepository GrantRepository { get; } = grantRepository;
    protected IPermissionManager Manager { get; } = manager;
    protected ILocalizableStringSerializer Serializer { get; } = serializer;


    public async Task<PagedResultDto<PermissionGrantInfoDto>> GetListAsync(PermissionGrantInfoRequestInput input)
    {
        using var tr = RecordRepository.DisableTracking();
        using var tg = GrantRepository.DisableTracking();

        var query = await RecordRepository.GetQueryableAsync();

        query = query
                .WhereIf(input.Root, u => u.ParentName == null)
                .WhereIf(!input.Prefix.IsNullOrEmpty(), u => u.Name.StartsWith(input.Prefix!))
                .WhereIf(!input.ProviderName.IsNullOrEmpty(), u =>
                    u.Providers == null || u.Providers == "" || u.Providers.Contains(input.ProviderName!))
            ;

        int count = query.Count();

        if (!input.Sorting.IsNullOrEmpty())
        {
            query = query.OrderBy(input.Sorting);
        }

        query = query.PageBy(input);

        var list = query.ToList();

        var res = new List<PermissionGrantInfoDto>();

        foreach (var item in list)
        {
            var l = Serializer.Deserialize(item.DisplayName);
            var displayName = l.Localize(StringLocalizerFactory);
            var dto = new PermissionGrantInfoDto
            {
                Name = item.Name,
                DisplayName = displayName,
                ParentName = item.ParentName
            };
            res.Add(dto);
        }

        if (input.ProviderName is not null && input.ProviderKey is not null & input.Prefix is not null)
        {
            var grantQuery = await GrantRepository.GetQueryableAsync();
            grantQuery = grantQuery
                .Where(u =>
                    u.Name.StartsWith(input.Prefix!) &&
                    u.ProviderKey == input.ProviderKey &&
                    u.ProviderName == input.ProviderName &&
                    u.TenantId == CurrentTenant.Id
                );
            var grants = grantQuery.Select(u => u.Name).ToList();

            var grantedRecords = res.IntersectBy(grants, u => u.Name);

            foreach (var record in grantedRecords)
            {
                record.IsGranted = true;
            }
        }

        return new PagedResultDto<PermissionGrantInfoDto>(count, res);
    }


    public virtual async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
    {
        var group = input.Permissions.GroupBy(u => u.IsGranted).ToList();
        var grantedPermissions = group.FirstOrDefault(u => u.Key)?
            .Select(u => u.Name).ToArray() ?? [];
        var deniedPermissions = group.FirstOrDefault(u => !u.Key)?
            .Select(u => u.Name).ToArray() ?? [];

        await Manager.UpdateAsync(providerName, providerKey, grantedPermissions, deniedPermissions);
    }
}