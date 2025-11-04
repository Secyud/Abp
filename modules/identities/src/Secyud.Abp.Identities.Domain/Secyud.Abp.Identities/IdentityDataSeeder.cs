using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Secyud.Abp.Identities;

public class IdentityDataSeeder(
    IGuidGenerator guidGenerator,
    IIdentityRoleRepository roleRepository,
    IIdentityUserRepository userRepository,
    ILookupNormalizer lookupNormalizer,
    IdentityUserManager userManager,
    IdentityRoleManager roleManager,
    ICurrentTenant currentTenant,
    IOptions<IdentityOptions> identityOptions)
    : ITransientDependency, IIdentityDataSeeder
{
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
    protected IIdentityRoleRepository RoleRepository { get; } = roleRepository;
    protected IIdentityUserRepository UserRepository { get; } = userRepository;
    protected ILookupNormalizer LookupNormalizer { get; } = lookupNormalizer;
    protected IdentityUserManager UserManager { get; } = userManager;
    protected IdentityRoleManager RoleManager { get; } = roleManager;
    protected ICurrentTenant CurrentTenant { get; } = currentTenant;
    protected IOptions<IdentityOptions> IdentityOptions { get; } = identityOptions;

    [UnitOfWork]
    public virtual async Task<IdentityDataSeedResult> SeedAsync(
        string adminEmail,
        string adminPassword,
        Guid? tenantId = null,
        string? adminUserName = null)
    {
        Check.NotNullOrWhiteSpace(adminEmail, nameof(adminEmail));
        Check.NotNullOrWhiteSpace(adminPassword, nameof(adminPassword));

        using (CurrentTenant.Change(tenantId))
        {
            await IdentityOptions.SetAsync();

            var result = new IdentityDataSeedResult();
            //"admin" user
            if(adminUserName.IsNullOrWhiteSpace())
            {
                adminUserName = IdentityDataSeedContributor.AdminUserNameDefaultValue;
            }
            var adminUser = await UserRepository.FindByNormalizedUserNameAsync(
                LookupNormalizer.NormalizeName(adminUserName)
            );

            if (adminUser != null)
            {
                return result;
            }

            adminUser = new IdentityUser(
                GuidGenerator.Create(),
                adminUserName,
                adminEmail,
                tenantId
            )
            {
                Name = adminUserName
            };

            (await UserManager.CreateAsync(adminUser, adminPassword, validatePassword: false)).CheckErrors();
            result.CreatedAdminUser = true;

            //"admin" role
            const string adminRoleName = "admin";
            var adminRole =
                await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName(adminRoleName));
            if (adminRole == null)
            {
                adminRole = new IdentityRole(
                    GuidGenerator.Create(),
                    adminRoleName,
                    tenantId
                )
                {
                    IsStatic = true,
                    IsPublic = true
                };

                (await RoleManager.CreateAsync(adminRole)).CheckErrors();
                result.CreatedAdminRole = true;
            }

            (await UserManager.AddToRoleAsync(adminUser, adminRoleName)).CheckErrors();

            return result;
        }
    }
}
