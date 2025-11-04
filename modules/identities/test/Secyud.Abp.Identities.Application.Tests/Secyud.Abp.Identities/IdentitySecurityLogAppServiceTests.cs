using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Users;
using Xunit;

namespace Secyud.Abp.Identities;

public class IdentitySecurityLogAppServiceTests : AbpIdentityApplicationTestBase
{
    private readonly IIdentitySecurityLogAppService _identitySecurityLogAppService;
    private readonly IIdentitySecurityLogRepository _identitySecurityLogRepository;
    private readonly IdentityTestData _testData;
    private ICurrentUser _currentUser = null!;
    private Guid _userJohnId;

    public IdentitySecurityLogAppServiceTests()
    {
        _identitySecurityLogAppService = GetRequiredService<IIdentitySecurityLogAppService>();
        _identitySecurityLogRepository = GetRequiredService<IIdentitySecurityLogRepository>();
        _testData = GetRequiredService<IdentityTestData>();
        _userJohnId = _testData.UserJohnId;
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _currentUser = Substitute.For<ICurrentUser>();
        _currentUser.Id.Returns(info => _userJohnId);
        services.AddSingleton(_currentUser);
    }

    [Fact]
    public async Task GetListAsync()
    {
        var logs = await _identitySecurityLogAppService.GetListAsync(new GetIdentitySecurityLogListInput());

        logs.TotalCount.ShouldBe(2);
        logs.Items.ShouldNotBeEmpty();
        logs.Items.ShouldContain(x => x.ApplicationName == "Test-ApplicationName" && x.UserId == _testData.UserJohnId);
        logs.Items.ShouldContain(x => x.ApplicationName == "Test-ApplicationName" && x.UserId == _testData.UserDavidId);
    }
}
