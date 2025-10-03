using Shouldly;
using Xunit;

namespace Secyud.Abp.Tenants;

public class TenantConnectionString_Tests
{
    [Theory]
    [InlineData("aaa")]
    [InlineData("bbb")]
    public void SetValue(string value)
    {
        var tenantConnectionString =
            new TenantConnectionString(Guid.NewGuid(), "MyConnString", "MyConnString-Value");
        tenantConnectionString.SetValue(value);
        tenantConnectionString.Value.ShouldBe(value);
    }
}
