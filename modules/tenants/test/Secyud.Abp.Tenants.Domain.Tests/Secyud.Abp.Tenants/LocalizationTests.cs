using Microsoft.Extensions.Localization;
using Secyud.Abp.Tenants.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace Secyud.Abp.Tenants;

public class LocalizationTests : AbpTenantsDomainTestBase
{
    private readonly IStringLocalizer<AbpTenantsResource> _stringLocalizer;

    public LocalizationTests()
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

        using (CultureHelper.Use("zh-Hans"))
        {
            _stringLocalizer["TenantDeletionConfirmationMessage"].Value
                .ShouldBe("租户'{0}'将被删除。您确认吗？");
        }
    }
}
