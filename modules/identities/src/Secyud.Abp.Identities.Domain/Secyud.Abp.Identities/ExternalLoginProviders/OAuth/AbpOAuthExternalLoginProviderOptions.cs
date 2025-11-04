using Volo.Abp.Security.Claims;

namespace Secyud.Abp.Identities.ExternalLoginProviders.OAuth;

public class AbpOAuthExternalLoginProviderOptions
{
    public string NameClaimType { get; set; } = AbpClaimTypes.Name;
    
    public string SurnameClaimType { get; set; } = AbpClaimTypes.SurName;
    
    public string EmailClaimType { get; set; } = AbpClaimTypes.Email;
    
    public string EmailConfirmedClaimType { get; set; } = AbpClaimTypes.EmailVerified;
    
    public string PhoneNumberClaimType { get; set; } = AbpClaimTypes.PhoneNumber;
    
    public string PhoneNumberConfirmedClaimType { get; set; } = AbpClaimTypes.PhoneNumberVerified;
    
    public string ProviderKeyClaimType { get; set; } = AbpClaimTypes.UserId;

    public bool CanObtainUserInfoWithoutPassword { get; set; } = false;
}