using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Secyud.Abp.Identities.Localization;
using Volo.Abp;

namespace Secyud.Abp.Identities
{
    public class AbpIdentityUserValidator(IStringLocalizer<AbpIdentitiesResource> localizer) : IUserValidator<IdentityUser>
    {
        protected IStringLocalizer<AbpIdentitiesResource> Localizer { get; } = localizer;

        public virtual async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            var describer = new IdentityErrorDescriber();

            Check.NotNull(manager, nameof(manager));
            Check.NotNull(user, nameof(user));

            var errors = new List<IdentityError>();

            var userName = await manager.GetUserNameAsync(user);
            if (userName == null)
            {
                errors.Add(describer.InvalidUserName(null));
            }
            else
            {
                var owner = await manager.FindByEmailAsync(userName);
                if (owner != null && !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "InvalidUserName",
                        Description = Localizer["Secyud.Abp.Identity:InvalidUserName", userName]
                    });
                }
            }

            var email = await manager.GetEmailAsync(user);
            if (email == null)
            {
                errors.Add(describer.InvalidEmail(null));
            }
            else
            {
                var owner = await manager.FindByNameAsync(email);
                if (owner != null && !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "InvalidEmail",
                        Description = Localizer["Secyud.Abp.Identity:InvalidEmail", email]
                    });
                }
            }

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
    }
}
