using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SecurityLog;
using Volo.Abp.Uow;

namespace Secyud.Abp.Identities;

[Dependency(ReplaceServices = true)]
public class IdentitySecurityLogStore(
    ILogger<IdentitySecurityLogStore> logger,
    IOptions<AbpSecurityLogOptions> securityLogOptions,
    IIdentitySecurityLogRepository identitySecurityLogRepository,
    IGuidGenerator guidGenerator,
    IUnitOfWorkManager unitOfWorkManager)
    : ISecurityLogStore, ITransientDependency
{
    public ILogger<IdentitySecurityLogStore> Logger { get; set; } = logger;

    protected AbpSecurityLogOptions SecurityLogOptions { get; } = securityLogOptions.Value;
    protected IIdentitySecurityLogRepository IdentitySecurityLogRepository { get; } = identitySecurityLogRepository;
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;
    protected IUnitOfWorkManager UnitOfWorkManager { get; } = unitOfWorkManager;

    public async Task SaveAsync(SecurityLogInfo securityLogInfo)
    {
        if (!SecurityLogOptions.IsEnabled)
        {
            return;
        }

        using var uow = UnitOfWorkManager.Begin(requiresNew: true);
        await IdentitySecurityLogRepository.InsertAsync(new IdentitySecurityLog(GuidGenerator, securityLogInfo));
        await uow.CompleteAsync();
    }
}
