using System.Security.Claims;

namespace JobBoardPlatform.Application.Common.Constants;

public class ClaimConstants
{
    //سطح دسترسی برای کارفرما
    public const string EmployerClaimType = "Employer";
    public const string IsApprovedClaimValue = "IsApproved";

    //سطح دسترسی برای کارجو
    public const string JobSeekerClaimType = "JobSeeker";
    public const string IsActiveClaimValue = "IsAcitve";
}
