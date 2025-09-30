using Shouldly;
using Xunit;

namespace Secyud.Abp.Settings;

public class SettingCacheItem_Tests
{
    [Fact]
    public void GetSettingNameFormCacheKeyOrNull()
    {
        var key = new SettingCacheKey("aaa", "bbb", "ccc");
        key.Name.ShouldBe("aaa");
    }
}
