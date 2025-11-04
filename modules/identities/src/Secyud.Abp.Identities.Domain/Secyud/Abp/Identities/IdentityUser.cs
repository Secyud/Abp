using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Secyud.Abp.Users;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace Secyud.Abp.Identities;

public class IdentityUser : FullAuditedAggregateRoot<Guid>, IUser, IHasEntityVersion
{
    public Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the username for this user.
    /// </summary>
    public string? UserName { get; protected internal set; }

    /// <summary>
    /// Gets or sets the normalized username for this user.
    /// </summary>
    [DisableAuditing]
    public string? NormalizedUserName { get; protected internal set; }

    /// <summary>
    /// Gets or sets the Name for the user.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the Surname for the user.
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// Gets or sets the email address for this user.
    /// </summary>
    public string? Email { get; protected internal set; }

    /// <summary>
    /// Gets or sets the normalized email address for this user.
    /// </summary>
    [DisableAuditing]
    public string? NormalizedEmail { get; protected internal set; }

    /// <summary>
    /// Gets or sets a flag indicating if a user has confirmed their email address.
    /// </summary>
    /// <value>True if the email address has been confirmed, otherwise false.</value>
    public bool EmailConfirmed { get; protected internal set; }

    /// <summary>
    /// Gets or sets a salted and hashed representation of the password for this user.
    /// </summary>
    [DisableAuditing]
    public string? PasswordHash { get; protected internal set; }

    /// <summary>
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    [DisableAuditing]
    public string? SecurityStamp { get; protected internal set; }

    public bool IsExternal { get; set; }

    /// <summary>
    /// Gets or sets a telephone number for the user.
    /// </summary>
    public string? PhoneNumber { get; protected internal set; }

    /// <summary>
    /// Gets or sets a flag indicating if a user has confirmed their telephone address.
    /// </summary>
    /// <value>True if the telephone number has been confirmed, otherwise false.</value>
    public bool PhoneNumberConfirmed { get; protected internal set; }

    /// <summary>
    /// Gets or sets a flag indicating if the user is active.
    /// </summary>
    public bool IsActive { get; protected internal set; }

    /// <summary>
    /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
    /// </summary>
    /// <value>True if 2fa is enabled, otherwise false.</value>
    public bool TwoFactorEnabled { get; protected internal set; }

    /// <summary>
    /// Gets or sets the date and time, in UTC, when any user lockout ends.
    /// </summary>
    /// <remarks>
    /// A value in the past means the user is not locked out.
    /// </remarks>
    public DateTimeOffset? LockoutEnd { get; protected internal set; }

    /// <summary>
    /// Gets or sets a flag indicating if the user could be locked out.
    /// </summary>
    /// <value>True if the user could be locked out, otherwise false.</value>
    public bool LockoutEnabled { get; protected internal set; }

    /// <summary>
    /// Gets or sets the number of failed login attempts for the current user.
    /// </summary>
    public int AccessFailedCount { get; protected internal set; }

    /// <summary>
    /// Should change password on next login.
    /// </summary>
    public bool ShouldChangePasswordOnNextLogin { get; protected internal set; }

    /// <summary>
    /// A version value that is increased whenever the entity is changed.
    /// </summary>
    public int EntityVersion { get; protected set; }

    /// <summary>
    /// Gets or sets the last password change time for the user.
    /// </summary>
    public DateTimeOffset? LastPasswordChangeTime { get; protected set; }

    /// <summary>
    /// Navigation property for the roles this user belongs to.
    /// </summary>
    public virtual List<IdentityUserRole> Roles { get; protected set; } = null!;

    /// <summary>
    /// Navigation property for the claims this user possesses.
    /// </summary>
    public virtual List<IdentityUserClaim> Claims { get; protected set; } = null!;

    /// <summary>
    /// Navigation property for this users login accounts.
    /// </summary>
    public virtual List<IdentityUserLogin> Logins { get; protected set; } = null!;

    /// <summary>
    /// Navigation property for this users tokens.
    /// </summary>
    public virtual List<IdentityUserToken> Tokens { get; protected set; } = null!;

    protected IdentityUser()
    {
    }

    public IdentityUser(Guid id, string? userName, string? email, Guid? tenantId = null) : base(id)
    {
        Check.NotNull(userName, nameof(userName));
        Check.NotNull(email, nameof(email));

        TenantId = tenantId;
        UserName = userName;
        NormalizedUserName = userName.ToUpperInvariant();
        Email = email;
        NormalizedEmail = email.ToUpperInvariant();
        IsActive = true;
        SecurityStamp = Guid.NewGuid().ToString();

        Roles = [];
        Claims = [];
        Logins = [];
        Tokens = [];
    }

    public virtual void AddRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (IsInRole(roleId))
        {
            return;
        }

        Roles.Add(new IdentityUserRole(Id, roleId, TenantId));
    }

    public virtual void RemoveRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (!IsInRole(roleId))
        {
            return;
        }

        Roles.RemoveAll(r => r.RoleId == roleId);
    }

    public virtual bool IsInRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        return Roles.Any(r => r.RoleId == roleId);
    }

    public virtual void AddClaim(IGuidGenerator guidGenerator, Claim claim)
    {
        Check.NotNull(guidGenerator, nameof(guidGenerator));
        Check.NotNull(claim, nameof(claim));

        Claims.Add(new IdentityUserClaim(guidGenerator.Create(), Id, claim, TenantId));
    }

    public virtual void AddClaims(IGuidGenerator guidGenerator, IEnumerable<Claim> claims)
    {
        Check.NotNull(guidGenerator, nameof(guidGenerator));
        Check.NotNull(claims, nameof(claims));

        foreach (var claim in claims)
        {
            AddClaim(guidGenerator, claim);
        }
    }

    public virtual IdentityUserClaim? FindClaim(Claim claim)
    {
        Check.NotNull(claim, nameof(claim));

        return Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);
    }

    public virtual void ReplaceClaim(Claim claim, Claim newClaim)
    {
        Check.NotNull(claim, nameof(claim));
        Check.NotNull(newClaim, nameof(newClaim));

        var userClaims = Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);
        foreach (var userClaim in userClaims)
        {
            userClaim.SetClaim(newClaim);
        }
    }

    public virtual void RemoveClaims(IEnumerable<Claim> claims)
    {
        Check.NotNull(claims, nameof(claims));

        foreach (var claim in claims)
        {
            RemoveClaim(claim);
        }
    }

    public virtual void RemoveClaim(Claim claim)
    {
        Check.NotNull(claim, nameof(claim));

        Claims.RemoveAll(c => c.ClaimValue == claim.Value && c.ClaimType == claim.Type);
    }

    public virtual void AddLogin(UserLoginInfo login)
    {
        Check.NotNull(login, nameof(login));

        Logins.Add(new IdentityUserLogin(Id, login, TenantId));
    }

    public virtual void RemoveLogin(string loginProvider, string providerKey)
    {
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));

        Logins.RemoveAll(userLogin =>
            userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey);
    }

    public virtual IdentityUserToken? FindToken(string loginProvider, string name)
    {
        return Tokens.FirstOrDefault(t => t.LoginProvider == loginProvider && t.Name == name);
    }

    public virtual void SetToken(string loginProvider, string name, string? value)
    {
        var token = FindToken(loginProvider, name);
        if (token == null)
        {
            Tokens.Add(new IdentityUserToken(Id, loginProvider, name, value, TenantId));
        }
        else
        {
            token.Value = value;
        }
    }

    public virtual void RemoveToken(string loginProvider, string name)
    {
        Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
    }

    /// <summary>
    /// Use <see cref="IdentityUserManager.ConfirmEmailAsync"/> for regular email confirmation.
    /// Using this skips the confirmation process and directly sets the <see cref="EmailConfirmed"/>.
    /// </summary>
    public virtual void SetEmailConfirmed(bool confirmed)
    {
        EmailConfirmed = confirmed;
    }

    public virtual void SetPhoneNumberConfirmed(bool confirmed)
    {
        PhoneNumberConfirmed = confirmed;
    }

    /// <summary>
    /// Normally use <see cref="IdentityUserManager.ChangePhoneNumberAsync"/> to change the phone number
    /// in the application code.
    /// This method is to directly set it with a confirmation information.
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="confirmed"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetPhoneNumber(string? phoneNumber, bool confirmed)
    {
        PhoneNumber = phoneNumber;
        PhoneNumberConfirmed = !phoneNumber.IsNullOrWhiteSpace() && confirmed;
    }

    public virtual void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }

    public virtual void SetShouldChangePasswordOnNextLogin(bool shouldChangePasswordOnNextLogin)
    {
        ShouldChangePasswordOnNextLogin = shouldChangePasswordOnNextLogin;
    }

    public virtual void SetLastPasswordChangeTime(DateTimeOffset? lastPasswordChangeTime)
    {
        LastPasswordChangeTime = lastPasswordChangeTime;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, UserName = {UserName}";
    }
}