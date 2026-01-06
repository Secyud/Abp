using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Permissions;

public class PermissionGrantInfoRequestInput : PagedAndSortedResultRequestDto
{
    public string? ProviderName { get; set; }
    public string? ProviderKey { get; set; }

    public string? Prefix { get; set; }

    public bool Root { get; set; }
}