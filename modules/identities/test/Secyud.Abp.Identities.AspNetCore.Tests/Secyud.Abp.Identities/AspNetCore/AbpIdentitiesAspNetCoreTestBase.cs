using System.Net;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;

namespace Secyud.Abp.Identities.AspNetCore;

public abstract class AbpIdentitiesAspNetCoreTestBase : AbpAspNetCoreIntegratedTestBase<AbpIdentityAspNetCoreTestStartup>
{
    protected virtual async Task<string> GetResponseAsStringAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        var response = await GetResponseAsync(url, expectedStatusCode);
        return await response.Content.ReadAsStringAsync();
    }

    protected virtual async Task<HttpResponseMessage> GetResponseAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        var response = await Client.GetAsync(url);
        response.StatusCode.ShouldBe(expectedStatusCode);
        return response;
    }
}
