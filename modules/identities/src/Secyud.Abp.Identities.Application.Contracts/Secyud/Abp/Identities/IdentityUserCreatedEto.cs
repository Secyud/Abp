using Volo.Abp.Domain.Entities.Events.Distributed;

namespace Secyud.Abp.Identities;

[Serializable]
public class IdentityUserCreatedEto : EtoBase
{
    public Guid Id { get; set; }
}
