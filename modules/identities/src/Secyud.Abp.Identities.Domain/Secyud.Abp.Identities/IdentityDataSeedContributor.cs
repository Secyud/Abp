using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Secyud.Abp.Identities;

public class IdentityDataSeedContributor(IIdentityDataSeeder identityDataSeeder) : IDataSeedContributor, ITransientDependency
{
    public const string AdminEmailPropertyName = "AdminEmail";
    public const string AdminEmailDefaultValue = "admin@abp.io";
    public const string AdminUserNamePropertyName = "AdminUserName";
    public const string AdminUserNameDefaultValue = "admin";
    public const string AdminPasswordPropertyName = "AdminPassword";
    public const string AdminPasswordDefaultValue = "1q2w3E*";

    protected IIdentityDataSeeder IdentityDataSeeder { get; } = identityDataSeeder;

    public virtual Task SeedAsync(DataSeedContext context)
    {
        return IdentityDataSeeder.SeedAsync(
            context[AdminEmailPropertyName] as string ?? AdminEmailDefaultValue,
            context[AdminPasswordPropertyName] as string ?? AdminPasswordDefaultValue,
            context.TenantId,
            context[AdminUserNamePropertyName] as string ?? AdminUserNameDefaultValue
        );
    }
}
