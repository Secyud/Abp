using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Tenants;

public class GetTenantsInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}
