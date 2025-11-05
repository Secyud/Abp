using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Secyud.Abp.Users;

public interface IUser : IAggregateRoot<Guid>, IMultiTenant, IHasExtraProperties
{
    string UserName { get; }

    string Email { get; }

    string? Name { get; }

    string? Surname { get; }

    bool IsActive { get; }

    bool EmailConfirmed { get; }

    string? PhoneNumber { get; }

    bool PhoneNumberConfirmed { get; }
}
