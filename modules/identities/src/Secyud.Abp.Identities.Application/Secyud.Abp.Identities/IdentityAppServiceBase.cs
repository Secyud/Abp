using Secyud.Abp.Identities.Localization;
using Volo.Abp.Application.Services;

namespace Secyud.Abp.Identities;

public abstract class IdentityAppServiceBase : ApplicationService
{
    protected IdentityAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpIdentityApplicationModule);
        LocalizationResource = typeof(AbpIdentitiesResource);
    }

    protected async Task<List<IdentityUserDto>> FillIdentityUserDtoAsync(IIdentityUserRepository identityUserRepository, IdentitiesTwoFactorManager identitiesTwoFactorManager, List<IdentityUser> users)
    {
        var userRoles = await identityUserRepository.GetRoleNamesAsync(users.Select(x => x.Id));
        var userDtos = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(users);

        var twoFactorEnabled = await identitiesTwoFactorManager.IsOptionalAsync();
        for (var i = 0; i < users.Count; i++)
        {
            userDtos[i].IsLockedOut = users[i].LockoutEnabled && (users[i].LockoutEnd != null && users[i].LockoutEnd > DateTime.UtcNow);
            if (!userDtos[i].IsLockedOut)
            {
                userDtos[i].LockoutEnd = null;
            }
            userDtos[i].SupportTwoFactor = twoFactorEnabled;
            var userRole = userRoles.FirstOrDefault(x => x.Id == users[i].Id);
            userDtos[i].RoleNames = userRole != null ? userRole.RoleNames.ToList() : [];
        }

        return userDtos;
    }
}
