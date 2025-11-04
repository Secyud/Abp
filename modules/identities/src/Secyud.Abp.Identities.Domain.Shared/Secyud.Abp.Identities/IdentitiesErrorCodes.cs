namespace Secyud.Abp.Identities;

public class IdentitiesErrorCodes
{
    public const string InvalidExternalLoginProvider = "Secyud.Abp.Identities:010010";
    public const string ExternalLoginProviderAuthenticateFailed = "Secyud.Abp.Identities:010011";
    public const string LocalUserAlreadyExists = "Secyud.Abp.Identities:010012";
    public const string NoUserFoundInFile = "Secyud.Abp.Identities:010013";
    public const string InvalidImportFileFormat = "Secyud.Abp.Identities:010014";
    public const string MaximumUserCount = "Secyud.Abp.Identities:010015";
    public const string UserSelfDeletion = "Secyud.Abp.Identities:010001";
    public const string MaxAllowedOuMembership = "Secyud.Abp.Identities:010002";
    public const string ExternalUserPasswordChange = "Secyud.Abp.Identities:010003";
    public const string StaticRoleRenaming = "Secyud.Abp.Identities:010005";
    public const string StaticRoleDeletion = "Secyud.Abp.Identities:010006";
    public const string UsersCanNotChangeTwoFactor = "Secyud.Abp.Identities:010007";
    public const string CanNotChangeTwoFactor = "Secyud.Abp.Identities:010008";
    public const string YouCannotDelegateYourself = "Secyud.Abp.Identities:010009";
    public const string ClaimNameExist = "Secyud.Abp.Identities:010021";
    public const string CanNotUpdateStaticClaimType = "Secyud.Abp.Identities:010022";
    public const string CanNotDeleteStaticClaimType = "Secyud.Abp.Identities:010023";
}
