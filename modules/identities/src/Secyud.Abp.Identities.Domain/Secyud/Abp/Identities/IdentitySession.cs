using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace Secyud.Abp.Identities;

public class IdentitySession : BasicAggregateRoot<Guid>, IHasExtraProperties, IMultiTenant
{
    public string SessionId { get; protected set; } = "";

    /// <summary>
    /// Web, Mobile ...
    /// </summary>
    public string? Device { get; protected set; } 

    public string? DeviceInfo { get; protected set; }

    public Guid? TenantId { get; protected set; }

    public Guid UserId { get; protected set; }

    public string ClientId { get; set; } = "";

    public string? IpAddresses { get; protected set; } 

    public DateTime SignedIn { get; protected set; }

    public DateTime? LastAccessed { get; protected set; }

    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; } = new();

    protected IdentitySession()
    {
        this.SetDefaultsForExtraProperties();
    }

    public IdentitySession(
        Guid id,
        string sessionId,
        string device,
        string deviceInfo,
        Guid userId,
        Guid? tenantId,
        string clientId,
        string ipAddresses,
        DateTime signedIn,
        DateTime? lastAccessed = null)
        : base(id)
    {
        SessionId = sessionId;
        Device = device;
        DeviceInfo = deviceInfo;
        UserId = userId;
        TenantId = tenantId;
        ClientId = clientId;
        IpAddresses = ipAddresses;
        SignedIn = signedIn;
        LastAccessed = lastAccessed;

        this.SetDefaultsForExtraProperties();
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return ExtensibleObjectValidator.GetValidationErrors(
            this,
            validationContext
        );
    }

    public void SetSignedInTime(DateTime signedIn)
    {
        SignedIn = signedIn;
    }

    public void UpdateLastAccessedTime(DateTime? lastAccessed)
    {
        LastAccessed = lastAccessed;
    }

    public void SetIpAddresses(IEnumerable<string> ipAddresses)
    {
        IpAddresses = JoinAsString(ipAddresses);
    }

    public IEnumerable<string> GetIpAddresses()
    {
        return GetArrayFromString(IpAddresses);
    }

    private static string? JoinAsString(IEnumerable<string> list)
    {
        var serialized = string.Join(",", list);
        if (serialized.IsNullOrWhiteSpace())
        {
            return null;
        }

        while (serialized.Length > IdentitySessionConsts.MaxIpAddressesLength)
        {
            var lastCommaIndex = serialized.IndexOf(',');
            if (lastCommaIndex < 0)
            {
                return serialized;
            }

            serialized = serialized.Substring(lastCommaIndex + 1);
        }

        return serialized;
    }

    private string[] GetArrayFromString(string? str)
    {
        return str?.Split(",", StringSplitOptions.RemoveEmptyEntries) ?? [];
    }
}