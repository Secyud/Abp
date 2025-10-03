namespace Secyud.Abp.Tenants;

public interface ITenantValidator
{
    Task ValidateAsync(Tenant tenant);
}
