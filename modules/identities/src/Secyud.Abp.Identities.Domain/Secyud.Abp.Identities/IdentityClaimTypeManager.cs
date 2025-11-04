using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Secyud.Abp.Identities;

public class IdentityClaimTypeManager(
    IIdentityClaimTypeRepository identityClaimTypeRepository,
    IIdentityUserRepository identityUserRepository,
    IIdentityRoleRepository identityRoleRepository)
    : DomainService
{
    protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; } = identityClaimTypeRepository;
    protected IIdentityUserRepository IdentityUserRepository { get; } = identityUserRepository;
    protected IIdentityRoleRepository IdentityRoleRepository { get; } = identityRoleRepository;

    public virtual async Task<IdentityClaimType> CreateAsync(IdentityClaimType claimType)
    {
        if (await IdentityClaimTypeRepository.AnyAsync(claimType.Name))
        {
            throw new BusinessException(IdentitiesErrorCodes.ClaimNameExist).WithData("0", claimType.Name);
        }

        return await IdentityClaimTypeRepository.InsertAsync(claimType);
    }

    public virtual async Task<IdentityClaimType> UpdateAsync(IdentityClaimType claimType)
    {
        if (await IdentityClaimTypeRepository.AnyAsync(claimType.Name, claimType.Id))
        {
            throw new BusinessException(IdentitiesErrorCodes.ClaimNameExist).WithData("0", claimType.Name);
        }

        if (claimType.IsStatic)
        {
            throw new BusinessException(IdentitiesErrorCodes.CanNotUpdateStaticClaimType);
        }

        return await IdentityClaimTypeRepository.UpdateAsync(claimType);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var claimType = await IdentityClaimTypeRepository.GetAsync(id);
        if (claimType.IsStatic)
        {
            throw new BusinessException(IdentitiesErrorCodes.CanNotDeleteStaticClaimType);
        }

        //Remove claim of this type from all users and roles
        await IdentityUserRepository.RemoveClaimFromAllUsersAsync(claimType.Name);
        await IdentityRoleRepository.RemoveClaimFromAllRolesAsync(claimType.Name);

        await IdentityClaimTypeRepository.DeleteAsync(id);
    }
}