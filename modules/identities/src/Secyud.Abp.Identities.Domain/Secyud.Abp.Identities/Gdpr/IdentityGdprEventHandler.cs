using System.Text;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Gdpr;
using Volo.Abp.Uow;

namespace Secyud.Abp.Identities.Gdpr;

public class IdentityGdprEventHandler(
    IdentityUserManager identityUserManager,
    IIdentityUserRepository identityUserRepository,
    IDistributedEventBus eventBus)
    :
        IDistributedEventHandler<GdprUserDataDeletionRequestedEto>,
        IDistributedEventHandler<GdprUserDataRequestedEto>,
        ITransientDependency
{
    protected IdentityUserManager IdentityUserManager { get; } = identityUserManager;
    protected IIdentityUserRepository IdentityUserRepository { get; } = identityUserRepository;
    protected IDistributedEventBus EventBus { get; } = eventBus;

    [UnitOfWork]
    public virtual async Task HandleEventAsync(GdprUserDataDeletionRequestedEto eventData)
    {
        var identityUser = await IdentityUserManager.GetByIdAsync(eventData.UserId);

        identityUser.Name = GenerateRandomText();
        identityUser.Surname = GenerateRandomText();
        identityUser.SetIsActive(false);

        await IdentityUserManager.SetPhoneNumberAsync(identityUser, null);
        await IdentityUserManager.SetUserNameAsync(identityUser, await GenerateUniqueUserNameAsync());
        await IdentityUserManager.SetEmailAsync(identityUser, await GenerateUniqueEmailAsync());

        foreach (var (key, _) in identityUser.ExtraProperties)
        {
            identityUser.ExtraProperties[key] = GenerateRandomText();
        }

        await IdentityUserRepository.UpdateAsync(identityUser);
        await IdentityUserRepository.DeleteAsync(identityUser);
    }

    public virtual async Task HandleEventAsync(GdprUserDataRequestedEto eventData)
    {
        var identityUser = await IdentityUserRepository.GetAsync(eventData.UserId);

        var gdprDataInfo = new GdprDataInfo
        {
            { "Username", identityUser.UserName ?? "" },
            { "Name", identityUser.Name ?? "" },
            { "Surname", identityUser.Surname ?? "" },
            { "Email", identityUser.Email ?? "" },
            { "Phone Number", identityUser.PhoneNumber ?? "" }
        };

        var claims = identityUser.Claims;
        foreach (var identityUserClaim in claims)
        {
            gdprDataInfo.Add(identityUserClaim.ClaimType, identityUserClaim.ClaimValue);
        }

        await EventBus.PublishAsync(
            new GdprUserDataPreparedEto
            {
                RequestId = eventData.RequestId,
                Data = gdprDataInfo,
                Provider = GetType().FullName!
            });
    }

    protected virtual string GenerateRandomText(int length = 8)
    {
        const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        var builder = new StringBuilder();
        for (var i = 0; i < length; i++)
        {
            builder.Append(letters[RandomHelper.GetRandom(letters.Length - 1)]);
        }

        return builder.ToString();
    }

    private async Task<string> GenerateUniqueUserNameAsync()
    {
        var userName = GenerateRandomText();

        while (await IdentityUserManager.FindByNameAsync(userName) != null)
        {
            userName = GenerateRandomText();
        }

        return userName;
    }

    private async Task<string> GenerateUniqueEmailAsync()
    {
        var email = GenerateRandomText() + "@xxx.xxx";

        while (await IdentityUserManager.FindByEmailAsync(email) != null)
        {
            email = GenerateRandomText() + "@xxx.xxx";
        }

        return email;
    }
}