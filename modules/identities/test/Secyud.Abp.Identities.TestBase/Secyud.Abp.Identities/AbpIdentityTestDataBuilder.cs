using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SecurityLog;

namespace Secyud.Abp.Identities;

public class AbpIdentityTestDataBuilder(
    IGuidGenerator guidGenerator,
    IIdentityUserRepository userRepository,
    IIdentityClaimTypeRepository identityClaimTypeRepository,
    IIdentityRoleRepository roleRepository,
    ILookupNormalizer lookupNormalizer,
    IdentityTestData testData,
    IIdentitySecurityLogRepository identitySecurityLogRepository)
    : ITransientDependency
{
    private IdentityRole _adminRole = null!;
    private IdentityRole _moderator = null!;
    private IdentityRole _supporterRole = null!;
    private IdentityRole _managerRole = null!;

    public async Task Build()
    {
        await AddRoles();
        await AddUsers();
        await AddClaimTypes();
        await AddSecurityLogs();
    }
    

    private async Task AddRoles()
    {
        _adminRole = (await roleRepository.FindByNormalizedNameAsync(lookupNormalizer.NormalizeName("admin")))!;

        _moderator = new IdentityRole(testData.RoleModeratorId, "moderator");
        _moderator.AddClaim(guidGenerator, new Claim("test-claim", "test-value"));
        await roleRepository.InsertAsync(_moderator);

        _supporterRole = new IdentityRole(guidGenerator.Create(), "supporter");
        await roleRepository.InsertAsync(_supporterRole);

        _managerRole = new IdentityRole(guidGenerator.Create(), "manager");
        await roleRepository.InsertAsync(_managerRole);
    }

    private async Task AddUsers()
    {
        var adminUser = new IdentityUser(guidGenerator.Create(), "administrator", "admin@abp.io");
        adminUser.AddRole(_adminRole.Id);
        adminUser.AddClaim(guidGenerator, new Claim("TestClaimType", "42"));
        await userRepository.InsertAsync(adminUser);

        var john = new IdentityUser(testData.UserJohnId, "john.nash", "john.nash@abp.io");
        john.AddRole(_moderator.Id);
        john.AddRole(_supporterRole.Id);
        john.AddLogin(new UserLoginInfo("github", "john", "John Nash"));
        john.AddLogin(new UserLoginInfo("twitter", "johnx", "John Nash"));
        john.AddClaim(guidGenerator, new Claim("TestClaimType", "42"));
        john.SetToken("test-provider", "test-name", "test-value");
        await userRepository.InsertAsync(john);

        var david = new IdentityUser(testData.UserDavidId, "david", "david@abp.io");
        await userRepository.InsertAsync(david);

        var neo = new IdentityUser(testData.UserNeoId, "neo", "neo@abp.io");
        neo.AddRole(_supporterRole.Id);
        neo.AddClaim(guidGenerator, new Claim("TestClaimType", "43"));
        await userRepository.InsertAsync(neo);
    }

    private async Task AddClaimTypes()
    {
        var ageClaim = new IdentityClaimType(testData.AgeClaimId, "Age", false, false, null, null, null, IdentityClaimValueType.Int);
        await identityClaimTypeRepository.InsertAsync(ageClaim);
        var educationClaim = new IdentityClaimType(testData.EducationClaimId, "Education", true, false, null, null, null);
        await identityClaimTypeRepository.InsertAsync(educationClaim);
        var socialNumberClaim = new IdentityClaimType(testData.SocialNumberClaimId, "SocialNumber", true, false, "^[0-9]*$", null, null, IdentityClaimValueType.String);
        await identityClaimTypeRepository.InsertAsync(socialNumberClaim);
    }

    private async Task AddSecurityLogs()
    {
        var johnLog = await identitySecurityLogRepository.InsertAsync(new IdentitySecurityLog(guidGenerator, new SecurityLogInfo
        {
            ApplicationName = "Test-ApplicationName",
            Identity = "Test-Identity",
            Action = "Test-Action",
            UserId = testData.UserJohnId,
            UserName = "john.nash",

            CreationTime = new DateTime(2020, 01, 01, 10, 0, 0)
        }));
        testData.UserJohnSecurityLogId = johnLog.Id;

        var davidLog = await identitySecurityLogRepository.InsertAsync(new IdentitySecurityLog(guidGenerator, new SecurityLogInfo
        {
            ApplicationName = "Test-ApplicationName",
            Identity = "Test-Identity",
            Action = "Test-Action",
            UserId = testData.UserDavidId,
            UserName = "david",

            CreationTime = new DateTime(2020, 01, 02, 10, 0, 0)
        }));
        testData.UserDavidSecurityLogId = davidLog.Id;
    }
}
