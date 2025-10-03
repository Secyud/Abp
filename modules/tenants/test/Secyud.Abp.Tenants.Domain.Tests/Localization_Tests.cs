using Microsoft.Extensions.Localization;
using Secyud.Abp.Tenants.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace Secyud.Abp.Tenants;

public class Localization_Tests : AbpTenantsDomainTestBase
{
    private readonly IStringLocalizer<AbpTenantsResource> _stringLocalizer;

    public Localization_Tests()
    {
        _stringLocalizer = GetRequiredService<IStringLocalizer<AbpTenantsResource>>();
    }

    [Fact]
    public void Test()
    {
        using (CultureHelper.Use("en"))
        {
            _stringLocalizer["TenantDeletionConfirmationMessage"].Value
                .ShouldBe("Tenant '{0}' will be deleted. Do you confirm that?");
        }

        using (CultureHelper.Use("en-gb"))
        {
            _stringLocalizer["TenantDeletionConfirmationMessage"].Value
                .ShouldBe("Tenant '{0}' will be deleted. Is that OK?");
        }

        using (CultureHelper.Use("tr"))
        {
            _stringLocalizer["TenantDeletionConfirmationMessage"].Value
                .ShouldBe("'{0}' isimli müşteri silinecektir. Onaylıyor musunuz?");
        }
    }
}
