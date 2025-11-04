namespace Secyud.Abp.Identities.Session;

public class IdentitySessionCheckerOptions
{
    /// <summary>
    /// The session in the database will be updated when cache hits reach this value.
    /// </summary>
    public int UpdateSessionAfterCacheHit { get; set; } = 10;
}
