namespace Secyud.Abp.Authorization;

public static class AbpAuthorizationErrorCodes
{
    public const string GivenPolicyHasNotGranted = "Secyud.Authorization:010001";

    public const string GivenPolicyHasNotGrantedWithPolicyName = "Secyud.Authorization:010002";

    public const string GivenPolicyHasNotGrantedForGivenResource = "Secyud.Authorization:010003";

    public const string GivenRequirementHasNotGrantedForGivenResource = "Secyud.Authorization:010004";

    public const string GivenRequirementsHasNotGrantedForGivenResource = "Secyud.Authorization:010005";
}
