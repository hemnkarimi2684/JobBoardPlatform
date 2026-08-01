namespace JobBoardPlatform.Application.Common.AccessClaims.UserClaim;

public class UserClaims
{
    //سطح دسترسی برای کارفرما
    public const string EmployerClaimType = "Employer";
    public const string IsApprovedClaimValue = "IsApproved";

    //سطح دسترسی برای کارجو
    public const string JobSeekerClaimType = "JobSeeker";
    public const string IsActiveClaimValue = "IsActive";
}
