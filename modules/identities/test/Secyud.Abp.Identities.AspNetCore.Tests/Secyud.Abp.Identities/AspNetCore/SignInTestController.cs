using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Secyud.Abp.Identities.AspNetCore;

[Route("api/signin-test")]
public class SignInTestController(SignInManager<IdentityUser> signInManager) : AbpController
{
    [Route("password")]
    public async Task<ActionResult> PasswordLogin(string userName, string password)
    {
        var result = await signInManager.PasswordSignInAsync(
            userName,
            password,
            false,
            false
        );

        return Content(result.ToString());
    }
}
