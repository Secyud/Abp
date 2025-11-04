namespace Secyud.Abp.Identities;

public class IdentityDynamicClaimsPrincipalContributorCacheOptions
{
    public TimeSpan CacheAbsoluteExpiration { get; set; } = TimeSpan.FromHours(1);
}
