using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

public class IdentityExternalLoginAppService(IdentityUserManager userManager) : IdentityAppServiceBase, IIdentityExternalLoginAppService
{
    protected IdentityUserManager UserManager { get; } = userManager;

    [Authorize]
    public virtual async Task CreateOrUpdateAsync()
    {
        var user = await UserManager.FindByIdAsync(CurrentUser.Id.ToString()!);
        if (user == null)
        {
            await CreateCurrentUserAsync();
        }
        else
        {
            await UpdateCurrentUserAsync(user);
        }
    }
    
    protected virtual async Task CreateCurrentUserAsync()
    {
        var user = new IdentityUser(
            CurrentUser.GetId(),
            CurrentUser.UserName, 
            CurrentUser.Email,
            CurrentTenant.Id
        );
        
        user.SetEmailConfirmed(CurrentUser.EmailVerified);

        if (!CurrentUser.PhoneNumber.IsNullOrEmpty())
        {
            user.SetPhoneNumber(CurrentUser.PhoneNumber, CurrentUser.PhoneNumberVerified);
        }
        
        user.Name = CurrentUser.Name;
        user.Surname = CurrentUser.SurName;
            
        (await UserManager.CreateAsync(user)).CheckErrors();
    }

    protected virtual async Task UpdateCurrentUserAsync(IdentityUser user)
    {
        if (!CurrentUser.UserName.IsNullOrWhiteSpace() &&
            !string.Equals(user.UserName, CurrentUser.UserName, StringComparison.InvariantCultureIgnoreCase))
        {
            await UserManager.SetUserNameAsync(user, CurrentUser.UserName);
        }

        if (!CurrentUser.Email.IsNullOrWhiteSpace() &&
            !string.Equals(user.Email, CurrentUser.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetEmailAsync(user, CurrentUser.Email)).CheckErrors();
            user.SetEmailConfirmed(CurrentUser.EmailVerified);
        }
        
        if (!CurrentUser.PhoneNumber.IsNullOrWhiteSpace() &&
            !string.Equals(user.PhoneNumber, CurrentUser.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetPhoneNumberAsync(user, CurrentUser.PhoneNumber)).CheckErrors();
            user.SetPhoneNumberConfirmed(CurrentUser.PhoneNumberVerified);
        }
        
        if (!CurrentUser.Name.IsNullOrWhiteSpace() &&
            !string.Equals(user.Name, CurrentUser.Name, StringComparison.InvariantCultureIgnoreCase))
        {
            user.Name = CurrentUser.Name;
        }
        
        if (!CurrentUser.SurName.IsNullOrWhiteSpace() &&
            !string.Equals(user.Surname, CurrentUser.SurName, StringComparison.InvariantCultureIgnoreCase))
        {
            user.Surname = CurrentUser.SurName;
        }
        
        (await UserManager.UpdateAsync(user)).CheckErrors();
    }
}