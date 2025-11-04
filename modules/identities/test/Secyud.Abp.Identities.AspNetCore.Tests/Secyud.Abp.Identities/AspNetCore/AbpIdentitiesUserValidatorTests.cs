using Microsoft.Extensions.Localization;
using Secyud.Abp.Identities.Localization;
using Shouldly;
using Xunit;

namespace Secyud.Abp.Identities.AspNetCore;

public class AbpIdentitiesUserValidatorTests : AbpIdentitiesAspNetCoreTestBase
{
    private readonly IdentityUserManager _identityUserManager;
    private readonly IStringLocalizer<AbpIdentitiesResource> _localizer;

    public AbpIdentitiesUserValidatorTests()
    {
        _identityUserManager = GetRequiredService<IdentityUserManager>();
        _localizer = GetRequiredService<IStringLocalizer<AbpIdentitiesResource>>();
    }

    [Fact]
    public async Task InvalidUserName_Messages_Test()
    {
        var user = new IdentityUser(Guid.NewGuid(), "abp 123", "user@google.com");
        var identityResult = await _identityUserManager.CreateAsync(user);
        identityResult.Succeeded.ShouldBeFalse();
        identityResult.Errors.Count().ShouldBe(1);
        identityResult.Errors.First().Code.ShouldBe("InvalidUserName");
        identityResult.Errors.First().Description.ShouldBe(_localizer["Secyud.Abp.Identity:InvalidUserName", "abp 123"]);
    }

    [Fact]
    public async Task Can_Not_Use_Another_Users_Email_As_Your_Username_Test()
    {
        var user1 = new IdentityUser(Guid.NewGuid(), "user1", "user1@google.com");
        var identityResult = await _identityUserManager.CreateAsync(user1);
        identityResult.Succeeded.ShouldBeTrue();

        var user2 = new IdentityUser(Guid.NewGuid(), "user1@google.com", "user2@google.com");
        identityResult = await _identityUserManager.CreateAsync(user2);
        identityResult.Succeeded.ShouldBeFalse();
        identityResult.Errors.Count().ShouldBe(1);
        identityResult.Errors.First().Code.ShouldBe("InvalidUserName");
        identityResult.Errors.First().Description.ShouldBe(_localizer["Secyud.Abp.Identity:InvalidUserName", "user1@google.com"]);
    }

    [Fact]
    public async Task Can_Not_Use_Another_Users_Name_As_Your_Email_Test()
    {
        var user1 = new IdentityUser(Guid.NewGuid(), "user1@google.com", "user@google.com");
        var identityResult = await _identityUserManager.CreateAsync(user1);
        identityResult.Succeeded.ShouldBeTrue();

        var user2 = new IdentityUser(Guid.NewGuid(), "user2", "user1@google.com");
        identityResult = await _identityUserManager.CreateAsync(user2);
        identityResult.Succeeded.ShouldBeFalse();
        identityResult.Errors.Count().ShouldBe(1);
        identityResult.Errors.First().Code.ShouldBe("InvalidEmail");
        identityResult.Errors.First().Description.ShouldBe(_localizer["Secyud.Abp.Identity:InvalidEmail", "user1@google.com"]);
    }
}
