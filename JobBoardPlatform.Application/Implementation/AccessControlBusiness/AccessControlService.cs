using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;


namespace JobBoardPlatform.Application.Implementation.AccessControlBusiness;

public class AccessControlService : IAccessControlService
{
    public void EnsureAdmin(ICurrentUser currentUser)
    {
        //ایا خود ادمینه که درخواست داده؟
        var isAdmin = IsAdmin(currentUser);
        if (isAdmin)
            return;

        //اگه هیچ کدوم نبود اکسپشن بده                                                               
        throw new ForbiddenException("You do not have sufficient access.");
    }

    public void EnsureApplicant(Guid applicantUserId, ICurrentUser currentUser)
    {
        //اگه کارجو درخواست داده ایا اطلاعات مربوط به خودش رو میبینه یا نه 
        var isApplicant = IsApplicant(applicantUserId, currentUser);
        if (isApplicant)
            return;

        //اگه هیچ کدوم نبود اکسپشن بده 
        throw new ForbiddenException("You do not have sufficient access.");
    }

    public void EnsureApplicantOrAdmin(Guid applicantUserId, ICurrentUser currentUser)
    {
        //اگه کارجو درخواست داده ایا اطلاعات مربوط به خودش رو میبینه یا نه 
        var isApplicant = IsApplicant(applicantUserId, currentUser);
        if (isApplicant)
            return;

        //ایا خود ادمینه که درخواست داده؟
        var isAdmin = IsAdmin(currentUser);
        if (isAdmin)
            return;

        //اگه هیچ کدوم نبود اکسپشن بده                                                               
        throw new ForbiddenException("You do not have sufficient access.");
    }

    public void EnsureApplicantOrOwnerEmployer(Guid ownerId, Guid applicantUserId, ICurrentUser currentUser)
    {
        //ایا کارفرماس؟ اگه هست اطلاعات مربوط به خودش رو مبینیه یا نه؟
        var isOwnerEmployer = IsOwnerEmployer(ownerId, currentUser);

        if (isOwnerEmployer)
            return;

        //اگه کارجو درخواست داده ایا اطلاعات مربوط به خودش رو میبینه یا نه 
        var isApplicant = IsApplicant(applicantUserId, currentUser);
        if (isApplicant)
            return;

        //اگه هیچ کدوم نبود اکسپشن بده 
        throw new ForbiddenException("You do not have sufficient access.");
    }

    public void EnsureOwnerEmployerOrAdmin(Guid ownerId, ICurrentUser currentUser)
    {
        //ایا خود ادمینه که درخواست داده؟
        var isAdmin = IsAdmin(currentUser);
        if (isAdmin)
            return;

        //ایا کارفرماس؟ اگه هست اطلاعات مربوط به خودش رو مبینیه یا نه؟
        var isOwnerEmployer = IsOwnerEmployer(ownerId, currentUser);

        if (isOwnerEmployer)
            return;

        throw new ForbiddenException("You do not have sufficient access.");
    }

    public void EnsureOwnerEmployer(Guid ownerId, ICurrentUser currentUser)
    {
        //ایا کارفرماس؟ اگه هست اطلاعات مربوط به خودش رو مبینیه یا نه؟
        var isOwnerEmployer = IsOwnerEmployer(ownerId, currentUser);

        if (isOwnerEmployer)
            return;

        throw new ForbiddenException("You do not have sufficient access.");
    }

    private static bool IsAdmin(ICurrentUser currentUser) => currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

    private static bool IsOwnerEmployer(Guid ownerId, ICurrentUser currentUser) => currentUser.UserId == ownerId
                                                                                   && currentUser.UserRoles.Contains(RoleConstants.EmployerRoleName);

    private static bool IsApplicant(Guid applicantUserId, ICurrentUser currentUser) => currentUser.UserId == applicantUserId;
}
