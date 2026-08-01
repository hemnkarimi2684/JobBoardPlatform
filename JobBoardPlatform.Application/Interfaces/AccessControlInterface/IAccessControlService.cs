using JobBoardPlatform.Application.Common.CurrentUser.Interface;

namespace JobBoardPlatform.Application.Interfaces.AccessControlInterface;

public interface IAccessControlService
{
    /// <summary>
    /// اطمینان از دسترسی ادمین
    /// </summary>
    /// <param name="currentUser"></param>
    void EnsureAdmin(ICurrentUser currentUser);

    /// <summary>
    /// اطمینان از دسترسی درخواست کننده 
    /// </summary>
    /// <param name="applicantUserId"></param>
    /// <param name="currentUser"></param>
    void EnsureApplicant(Guid applicantUserId, ICurrentUser currentUser);

    /// <summary>
    /// اطمینان از مالک، کارفرما یا ادمین بودن
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="currentUser"></param>
    /// <param name="resourceName"></param>
    void EnsureOwnerEmployerOrAdmin(Guid ownerId, ICurrentUser currentUser);

    /// <summary>
    /// اطمینان از اینکه درخواست ‌کننده یا ادمین است
    /// </summary>
    /// <param name="applicantUserId"></param>
    /// <param name="currentUser"></param>
    /// <param name="resourceName"></param>
    void EnsureApplicantOrAdmin(Guid applicantUserId, ICurrentUser currentUser);

    /// <summary>
    /// اطمینان از مالک کارفرما
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="currentUser"></param>
    void EnsureOwnerEmployer(Guid ownerId, ICurrentUser currentUser);

    /// <summary>
    /// اطمینان از دسترسی به دیدن درخواست شغلی
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="applicantUserId"></param>
    /// <param name="currentUser"></param>
    void EnsureApplicantOrOwnerEmployer(Guid ownerId, Guid applicantUserId, ICurrentUser currentUser);
}
