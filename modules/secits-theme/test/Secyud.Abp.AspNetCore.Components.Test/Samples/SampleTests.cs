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
        var res = Uri.TryCreate(u, "ac?a=21&b=1200", out var uri2);

        var uri3 = new Uri("https://172.168.0.2:9000/ac?a=21&b=1200");
        var boo = uri3 == uri2;

        int boo2 = Uri.Compare(uri3, uri2, UriComponents.PathAndQuery, UriFormat.UriEscaped, StringComparison.InvariantCultureIgnoreCase);
        output.WriteLine(boo.ToString());
        output.WriteLine(boo2.ToString());
        return Task.CompletedTask;
    }
}