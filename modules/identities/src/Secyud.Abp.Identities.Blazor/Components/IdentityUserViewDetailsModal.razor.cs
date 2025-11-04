using Microsoft.AspNetCore.Components;
using Secyud.Secits.Blazor.Element;

namespace Secyud.Abp.Identities.Components;

public partial class IdentityUserViewDetailsModal
{
    protected string DefaultDateTimeFormat { get; set; } = "MMMM dd, yyyy — HH:mm";
    protected string DefaultEmptyValue { get; set; } = "-";

    protected SModal ViewDetailsModal = null!;

    [Inject]
    protected IIdentityUserAppService UserAppService { get; set; } = null!;

    protected Guid UserId { get; set; }

    protected string? CreatedBy { get; set; }

    protected string? ModifiedBy { get; set; }

    protected IdentityUserDto User { get; set; } = new();

    protected virtual async Task GetUserAsync()
    {
        User = await UserAppService.GetAsync(UserId);
        CreatedBy = await GetUserNameOrNullAsync(User.CreatorId);
        ModifiedBy = await GetUserNameOrNullAsync(User.LastModifierId);
    }

    public virtual async Task OpenAsync(Guid id)
    {
        UserId = id;
        await GetUserAsync();
        await ViewDetailsModal.ShowAsync();
    }

    protected async Task CloseViewDetailsModalAsync()
    {
        await ViewDetailsModal.HideAsync();
    }

    protected virtual async Task<string?> GetUserNameOrNullAsync(Guid? userId)
    {
        if (!userId.HasValue)
        {
            return null;
        }

        var user = await UserAppService.FindByIdAsync(userId.Value);
        return user?.UserName;
    }

    protected virtual string ConvertUserFriendlyFormat(DateTime? dateTime)
    {
        return dateTime == null ? DefaultEmptyValue : dateTime.Value.ToUniversalTime().ToString(DefaultDateTimeFormat);
    }

    protected virtual string ConvertUserFriendlyFormat(DateTimeOffset? dateTime)
    {
        return dateTime == null ? DefaultEmptyValue : dateTime.Value.UtcDateTime.ToString(DefaultDateTimeFormat);
    }

    protected virtual string ConvertUserFriendlyFormat(string? value)
    {
        return value.IsNullOrWhiteSpace() ? DefaultEmptyValue : value;
    }

    protected virtual string ConvertUserFriendlyFormat(bool value)
    {
        return value ? L["Yes"].Value : L["No"].Value;
    }
}