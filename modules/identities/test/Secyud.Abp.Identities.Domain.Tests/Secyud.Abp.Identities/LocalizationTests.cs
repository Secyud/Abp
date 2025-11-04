using Microsoft.Extensions.Localization;
using Secyud.Abp.Identities.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace Secyud.Abp.Identities;

public class LocalizationTests : AbpIdentityDomainTestBase
{
    [Fact]
    public void Localization_Test()
    {
        using (CultureHelper.Use("en"))
        {
            GetRequiredService<IStringLocalizer<AbpIdentitiesResource>>()["Permission:IdentityManagement"].Value.ShouldBe("Identity management");
        }
    }
}
