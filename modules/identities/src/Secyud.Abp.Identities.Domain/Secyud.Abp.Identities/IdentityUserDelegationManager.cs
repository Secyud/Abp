using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Secyud.Abp.Identities;

public class IdentityUserDelegationManager(IIdentityUserDelegationRepository identityUserDelegationRepository) : DomainService
{
    protected IIdentityUserDelegationRepository IdentityUserDelegationRepository { get; } = identityUserDelegationRepository;

    public virtual async Task<List<IdentityUserDelegation>> GetListAsync(Guid? sourceUserId = null, Guid? targetUserId = null, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.GetListAsync(sourceUserId, targetUserId, cancellationToken: cancellationToken);
    }
    
    public virtual async Task<List<IdentityUserDelegation>> GetActiveDelegationsAsync(Guid targetUseId, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.GetActiveDelegationsAsync(targetUseId, cancellationToken: cancellationToken);
    }

    public virtual async Task<IdentityUserDelegation?> FindActiveDelegationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.FindActiveDelegationByIdAsync(id, cancellationToken: cancellationToken);
    }

    public virtual async Task DelegateNewUserAsync(Guid sourceUserId, Guid targetUserId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default)
    {
        if (sourceUserId == targetUserId)
        {
            throw new BusinessException(IdentitiesErrorCodes.YouCannotDelegateYourself);
        }
        
        await IdentityUserDelegationRepository.InsertAsync(
            new IdentityUserDelegation(
                GuidGenerator.Create(),
                sourceUserId,
                targetUserId,
                startTime,
                endTime,
                CurrentTenant.Id
            ),
            cancellationToken: cancellationToken
        );
    }

    public virtual async Task DeleteDelegationAsync(Guid id, Guid sourceUserId, CancellationToken cancellationToken = default)
    {
        var delegation = await IdentityUserDelegationRepository.FindAsync(id, cancellationToken: cancellationToken);

        if (delegation != null && delegation.SourceUserId == sourceUserId)
        {
            await IdentityUserDelegationRepository.DeleteAsync(delegation, cancellationToken: cancellationToken);
        }
    }
}
