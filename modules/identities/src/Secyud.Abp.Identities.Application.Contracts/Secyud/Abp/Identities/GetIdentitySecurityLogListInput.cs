using Volo.Abp.Application.Dtos;

namespace Secyud.Abp.Identities;

public class GetIdentitySecurityLogListInput : ExtensiblePagedAndSortedResultRequestDto
{
    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? ApplicationName { get; set; }

    public string? Identity { get; set; }

    public string? Action { get; set; }

    public string? UserName { get; set; }

    public string? ClientId { get; set; }

    public string? CorrelationId { get; set; }

    public string? ClientIpAddress { get; set; }
}