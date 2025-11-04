using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Secyud.Abp.Identities.AspNetCore;

public class LinkUserTokenProvider(
    IDataProtectionProvider dataProtectionProvider,
    IOptions<DataProtectionTokenProviderOptions> options,
    ILogger<DataProtectorTokenProvider<IdentityUser>> logger)
    : DataProtectorTokenProvider<IdentityUser>(dataProtectionProvider, options, logger);
