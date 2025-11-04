using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace Secyud.Abp.Identities;

public class AbpIdentityResultExceptionTests : AbpIdentityDomainTestBase
{
    [Fact]
    public void Should_Localize_Messages()
    {
        var exception = new AbpIdentityResultException(
            IdentityResult.Failed(
                new IdentityError
                {
                    Code = "PasswordTooShort",
                    Description = "Passwords must be at least 6 characters."
                },
                new IdentityError
                {
                    Code = "PasswordRequiresNonAlphanumeric",
                    Description = "Passwords must have at least one non alphanumeric character."
                }
            )
        );

        using (CultureHelper.Use("tr"))
        {
            var localizeMessage = exception.LocalizeMessage(new LocalizationContext(ServiceProvider));

            localizeMessage.ShouldContain("Şifre en az 6 karakter uzunluğunda olmalı.");
            localizeMessage.ShouldContain("Şifre en az bir sayı ya da harf olmayan karakter içermeli.");
        }
    }
}
