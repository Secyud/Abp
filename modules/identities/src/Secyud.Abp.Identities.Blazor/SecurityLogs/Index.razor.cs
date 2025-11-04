using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Secyud.Abp.Identities.Components;
using Secyud.Abp.Identities.Navigation;
using Secyud.Secits.Blazor;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;

namespace Secyud.Abp.Identities.SecurityLogs;

[Route(IdentitiesMenus.SecurityLogsUrl)]
[Authorize(IdentityPermissions.SecurityLogs.DefaultName)]
public partial class Index
{
    [Inject]
    protected IIdentitySecurityLogAppService AppService { get; set; } = null!;

    protected GetIdentitySecurityLogListInput Filter = new();
    protected SIteratorBase<IdentitySecurityLogDto>? View { get; set; }

    protected virtual async Task SearchEntitiesAsync()
    {
        await RefreshEntitiesAsync(true);
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task RefreshEntitiesAsync(bool resetState)
    {
        try
        {
            if (View is not null)
            {
                await View.RefreshAsync(resetState);
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task UpdateGetListInputAsync(DataRequest request)
    {
        if (Filter is ISortedResultRequest sortedResultRequestInput)
        {
            sortedResultRequestInput.Sorting = request.Sorters
                .Where(u => u.Enabled)
                .OrderBy(u => u.Index)
                .Select(u => u.Field + (u.Desc ? " DESC" : ""))
                .JoinAsString(", ");
        }

        if (Filter is IPagedResultRequest pagedResultRequestInput)
        {
            pagedResultRequestInput.SkipCount = request.SkipCount;
        }

        if (Filter is ILimitedResultRequest limitedResultRequestInput)
        {
            limitedResultRequestInput.MaxResultCount = request.PageSize;
        }

        Filter.ApplicationName = Filter.ApplicationName?.Trim();
        Filter.Identity = Filter.Identity?.Trim();
        Filter.UserName = Filter.UserName?.Trim();
        Filter.ClientId = Filter.ClientId?.Trim();
        Filter.CorrelationId = Filter.CorrelationId?.Trim();
        Filter.ClientIpAddress = Filter.ClientIpAddress?.Trim();

        return Task.CompletedTask;
    }

    protected virtual async Task ClearFilterAsync()
    {
        Filter = new GetIdentitySecurityLogListInput();
        await RefreshEntitiesAsync(true);
    }

    protected virtual async Task<DataResult<IdentitySecurityLogDto>> GetEntitiesAsync(DataRequest request)
    {
        await UpdateGetListInputAsync(request);
        var result = await AppService.GetListAsync(Filter);

        return new DataResult<IdentitySecurityLogDto>()
        {
            Items = result.Items,
            TotalCount = (int)result.TotalCount
        };
    }
}