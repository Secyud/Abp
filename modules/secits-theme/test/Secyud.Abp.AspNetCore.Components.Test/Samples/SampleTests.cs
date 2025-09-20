using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Secyud.Abp.AspNetCore.Samples;

public class SampleTests(ITestOutputHelper output) : AbpAspNetCoreComponentsTestBase
{
    [Fact]
    public Task Method1Async()
    {
        var uri = new Uri("https://172.168.0.1:9000/ac?a=21&b=1200");

        var u = new Uri($"{uri.Scheme}://{uri.Authority}");
        var res = Uri.TryCreate(u, "ac?b=1200&a=21", out var uri2);
  
        var boo = uri == uri2;
        uri.ShouldBeEquivalentTo(uri2);
        return Task.CompletedTask;
    }
}