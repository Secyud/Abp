using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Secyud.Abp.Identities.Settings;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Secyud.Abp.Identities;

public class IdentityUserManager(
    IdentityUserStore store,
    IIdentityRoleRepository roleRepository,
    IIdentityUserRepository userRepository,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<IdentityUser> passwordHasher,
    IEnumerable<IUserValidator<IdentityUser>> userValidators,
    IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<IdentityUserManager> logger,
    ICancellationTokenProvider cancellationTokenProvider,
    ISettingProvider settingProvider,
    IDistributedEventBus distributedEventBus,
    IIdentityLinkUserRepository identityLinkUserRepository,
    IDistributedCache<AbpDynamicClaimCacheItem> dynamicClaimCache)
    : UserManager<IdentityUser>(store, optionsAccessor, passwordHasher, userValidators,
        passwordValidators, keyNormalizer, errors, services, logger), IDomainService
{
    protected IIdentityRoleRepository RoleRepository { get; } = roleRepository;
    protected IIdentityUserRepository UserRepository { get; } = userRepository;
    protected ISettingProvider SettingProvider { get; } = settingProvider;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;
    protected IIdentityLinkUserRepository IdentityLinkUserRepository { get; } = identityLinkUserRepository;
    protected IDistributedCache<AbpDynamicClaimCacheItem> DynamicClaimCache { get; } = dynamicClaimCache;
    protected override CancellationToken CancellationToken => CancellationTokenProvider.Token;

    public virtual async Task<IdentityResult> CreateAsync(IdentityUser user, string password, bool validatePassword)
    {
        var result = await UpdatePasswordHash(user, password, validatePassword);
        if (!result.Succeeded)
        {
            return result;
        }

        return await CreateAsync(user);
    }

    public override async Task<IdentityResult> DeleteAsync(IdentityUser user)
    {
        user.Claims.Clear();
        user.Roles.Clear();
        user.Tokens.Clear();
        user.Logins.Clear();
        await IdentityLinkUserRepository.DeleteAsync(new IdentityLinkUserInfo(user.Id, user.TenantId), CancellationToken);
        await UpdateAsync(user);

        return await base.DeleteAsync(user);
    }

    protected override async Task<IdentityResult> UpdateUserAsync(IdentityUser user)
    {
        var result = await base.UpdateUserAsync(user);

        if (result.Succeeded)
        {
            await DynamicClaimCache.RemoveAsync(AbpDynamicClaimCacheItem.CalculateCacheKey(user.Id, user.TenantId), token: CancellationToken);
        }

        return result;
    }

    /// <summary>
    /// This is to call the protection method ValidateUserAsync
    /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
    /// called before saving the user via Create or Update.
    /// </summary>
    /// <param name="user">The user</param>
    /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
    public virtual async Task<IdentityResult> CallValidateUserAsync(IdentityUser user)
    {
        return await ValidateUserAsync(user);
    }

    /// <summary>
    /// This is to call the protection method ValidatePasswordAsync
    /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
    /// called before updating the password hash.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="password">The password.</param>
    /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
    public virtual async Task<IdentityResult> CallValidatePasswordAsync(IdentityUser user, string password)
    {
        return await ValidatePasswordAsync(user, password);
    }

    public virtual async Task<IdentityUser> GetByIdAsync(Guid id)
    {
        var user = await Store.FindByIdAsync(id.ToString(), CancellationToken);
        return user ?? throw new EntityNotFoundException(typeof(IdentityUser), id);
    }

    public virtual async Task<IdentityResult> SetRolesAsync(IdentityUser user, IEnumerable<string> roleNames)
    {
        Check.NotNull(user, nameof(user));
        Check.NotNull(roleNames, nameof(roleNames));

        var currentRoleNames = await GetRolesAsync(user);

        var result = await RemoveFromRolesAsync(user, currentRoleNames.Except(roleNames).Distinct());
        if (!result.Succeeded)
        {
            return result;
        }

        result = await AddToRolesAsync(user, roleNames.Except(currentRoleNames).Distinct());

        return !result.Succeeded ? result : IdentityResult.Success;
    }

    public virtual async Task<IdentityResult> AddDefaultRolesAsync(IdentityUser user)
    {
        await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, CancellationToken);

        foreach (var role in await RoleRepository.GetDefaultOnesAsync(cancellationToken: CancellationToken))
        {
            if (!user.IsInRole(role.Id))
            {
                user.AddRole(role.Id);
            }
        }

        return await UpdateUserAsync(user);
    }

    public virtual async Task<bool> ShouldPeriodicallyChangePasswordAsync(IdentityUser user)
    {
        Check.NotNull(user, nameof(user));

        if (user.PasswordHash.IsNullOrWhiteSpace())
        {
            return false;
        }

        var forceUsersToPeriodicallyChangePassword =
            await SettingProvider.GetAsync<bool>(IdentitiesSettingNames.Password.ForceUsersToPeriodicallyChangePassword);
        if (!forceUsersToPeriodicallyChangePassword)
        {
            return false;
        }

        var lastPasswordChangeTime = user.LastPasswordChangeTime ?? DateTime.SpecifyKind(user.CreationTime, DateTimeKind.Utc);
        var passwordChangePeriodDays = await SettingProvider.GetAsync<int>(IdentitiesSettingNames.Password.PasswordChangePeriodDays);

        return passwordChangePeriodDays > 0 && lastPasswordChangeTime.AddDays(passwordChangePeriodDays) < DateTime.UtcNow;
    }

    public virtual async Task ResetRecoveryCodesAsync(IdentityUser user)
    {
        if (!(Store is IdentityUserStore identityUserStore))
        {
            throw new AbpException($"Store is not an instance of {typeof(IdentityUserStore).AssemblyQualifiedName}");
        }

        await identityUserStore.SetTokenAsync(user, await identityUserStore.GetInternalLoginProviderAsync(),
            await identityUserStore.GetRecoveryCodeTokenNameAsync(), string.Empty, CancellationToken);
    }

    public override async Task<IdentityResult> SetEmailAsync(IdentityUser user, string? email)
    {
        var oldMail = user.Email;

        var result = await base.SetEmailAsync(user, email);

        result.CheckErrors();

        if (!string.IsNullOrEmpty(oldMail) && !oldMail.Equals(email, StringComparison.OrdinalIgnoreCase))
        {
            await DistributedEventBus.PublishAsync(
                new IdentityUserEmailChangedEto
                {
                    Id = user.Id,
                    TenantId = user.TenantId,
                    Email = email,
                    OldEmail = oldMail
                });
        }

        return result;
    }

    public override async Task<IdentityResult> SetUserNameAsync(IdentityUser user, string? userName)
    {
        var oldUserName = user.UserName;

        var result = await base.SetUserNameAsync(user, userName);

        result.CheckErrors();

        if (!string.IsNullOrEmpty(oldUserName) && oldUserName != userName)
        {
            await DistributedEventBus.PublishAsync(
                new IdentityUserUserNameChangedEto
                {
                    Id = user.Id,
                    TenantId = user.TenantId,
                    UserName = userName,
                    OldUserName = oldUserName
                });
        }

        return result;
    }

    public virtual async Task UpdateRoleAsync(Guid sourceRoleId, Guid? targetRoleId)
    {
        var sourceRole = await RoleRepository.GetAsync(sourceRoleId, cancellationToken: CancellationToken);

        Logger.LogDebug("Remove dynamic claims cache for users of role: {SourceRoleId}", sourceRoleId);
        var userIdList = await UserRepository.GetUserIdListByRoleIdAsync(sourceRoleId, cancellationToken: CancellationToken);
        await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, sourceRole.TenantId)),
            token: CancellationToken);

        var targetRole = targetRoleId.HasValue ? await RoleRepository.GetAsync(targetRoleId.Value, cancellationToken: CancellationToken) : null;
        if (targetRole != null)
        {
            Logger.LogDebug("Remove dynamic claims cache for users of role: {TargetRoleId}", targetRoleId);
            userIdList = await UserRepository.GetUserIdListByRoleIdAsync(targetRole.Id, cancellationToken: CancellationToken);
            await DynamicClaimCache.RemoveManyAsync(userIdList.Select(userId => AbpDynamicClaimCacheItem.CalculateCacheKey(userId, targetRole.TenantId)),
                token: CancellationToken);
        }

        await UserRepository.UpdateRoleAsync(sourceRoleId, targetRoleId, CancellationToken);
    }

    public virtual async Task<bool> ValidateUserNameAsync(string userName, Guid? userId = null)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(Options.User.AllowedUserNameCharacters) && userName.Any(c => !Options.User.AllowedUserNameCharacters.Contains(c)))
        {
            return false;
        }

        var owner = await FindByNameAsync(userName);
        if (owner != null && owner.Id != userId)
        {
            return false;
        }

        return true;
    }

    public virtual Task<string> GetRandomUserNameAsync(int length)
    {
        var allowedUserNameCharacters = Options.User.AllowedUserNameCharacters;
        if (allowedUserNameCharacters.IsNullOrWhiteSpace())
        {
            allowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        }

        var randomUserName = string.Empty;
        var random = new Random();
        while (randomUserName.Length < length)
        {
            randomUserName += allowedUserNameCharacters[random.Next(0, allowedUserNameCharacters.Length)];
        }

        return Task.FromResult(randomUserName);
    }

    public virtual async Task<string> GetUserNameFromEmailAsync(string email)
    {
        const int maxTryCount = 20;
        var tryCount = 0;

        var userName = email.Split('@')[0];

        if (await ValidateUserNameAsync(userName))
        {
            // The username is valid.
            return userName;
        }

        if (Options.User.AllowedUserNameCharacters.IsNullOrWhiteSpace())
        {
            // The AllowedUserNameCharacters is not set. So, we are generating a random username.
            tryCount = 0;
            do
            {
                var randomUserName = userName + RandomHelper.GetRandom(1000, 9999);
                if (await ValidateUserNameAsync(randomUserName))
                {
                    return randomUserName;
                }

                tryCount++;
            } while (tryCount < maxTryCount);
        }
        else if (!userName.All(Options.User.AllowedUserNameCharacters.Contains))
        {
            // The username contains not allowed characters. So, we are generating a random username.
            do
            {
                var randomUserName = await GetRandomUserNameAsync(userName.Length);
                if (await ValidateUserNameAsync(randomUserName))
                {
                    return randomUserName;
                }

                tryCount++;
            } while (tryCount < maxTryCount);
        }
        else if (Options.User.AllowedUserNameCharacters.Where(char.IsDigit).Distinct().Count() >= 4)
        {
            // The AllowedUserNameCharacters includes 4 numbers. So, we are generating 4 random numbers and appending to the username.
            var numbers = Options.User.AllowedUserNameCharacters.Where(char.IsDigit).OrderBy(x => Guid.NewGuid()).Take(4).ToArray();
            var minArray = numbers.OrderBy(x => x).ToArray();
            if (minArray[0] == '0')
            {
                var secondItem = minArray[1];
                minArray[0] = secondItem;
                minArray[1] = '0';
            }

            var min = int.Parse(new string(minArray));
            var max = int.Parse(new string(numbers.OrderByDescending(x => x).ToArray()));
            tryCount = 0;
            do
            {
                var randomUserName = userName + RandomHelper.GetRandom(min, max);
                if (await ValidateUserNameAsync(randomUserName))
                {
                    return randomUserName;
                }

                tryCount++;
            } while (tryCount < maxTryCount);
        }
        else
        {
            tryCount = 0;
            do
            {
                // The AllowedUserNameCharacters does not include numbers. So, we are generating 4 random characters and appending to the username.
                var randomUserName = userName + await GetRandomUserNameAsync(4);
                if (await ValidateUserNameAsync(randomUserName))
                {
                    return randomUserName;
                }

                tryCount++;
            } while (tryCount < maxTryCount);
        }

        Logger.LogError(
            "Could not get a valid user name for the given email address: {Email}, " +
            "allowed characters: {UserAllowedUserNameCharacters}, tried {MaxTryCount} times.",
            email, Options.User.AllowedUserNameCharacters, maxTryCount);
        throw new AbpIdentityResultException(IdentityResult.Failed(new IdentityErrorDescriber().InvalidUserName(userName)));
    }
}