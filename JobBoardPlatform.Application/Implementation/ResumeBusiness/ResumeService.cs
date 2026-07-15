using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.ResumeBusiness;

public class ResumeService : IResumeService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    public ResumeService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<bool> CreateResumeAsync(CreateResumeRequestDto resumeCommand)
    {
        var isExistUser = await _unitOfWork.UserRepository.IsUserExistAsync(resumeCommand.UserId);

        if (!isExistUser)
            throw new NotFoundException($"the user with id {resumeCommand.UserId} was not found");

        CheckSelfOrAdminPermission(resumeCommand.UserId, _currentUser);

        var isDuplicateResumeFortUser = await _unitOfWork.ResumeRepository.IsDuplicateResumeForUserAsync(resumeCommand.UserId);

        if (isDuplicateResumeFortUser)
            throw new ConflictException($"the user with id {resumeCommand.UserId} already has resume");

        var hasEducationDetail = await _unitOfWork.EducationDetailRepository.UserHasEducationDetailAsync(resumeCommand.UserId);

        if (!hasEducationDetail)
            throw new ValidationException("the user must have education detail for register resume");

        var resume = new Resume(resumeCommand.Title, resumeCommand.UserId, null, _currentUser.UserId);

        await _unitOfWork.ResumeRepository.AddAsync(resume);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<ResumeDetailResponseDto> GetResumeByUserIdAsync(Guid userId)
    {
        CheckSelfOrAdminPermission(userId, _currentUser);

        var result = await _unitOfWork.ResumeRepository.GetResumeByUserIdAsync(r => new ResumeDetailResponseDto
                                                                              (
                                                                                 r.Title,
                                                                                 r.UserId
                                                                              ), userId);

        if (result == null)
            throw new NotFoundException($"the resume with id {userId} was not found");

        return result;
    }

    private void CheckSelfOrAdminPermission(Guid? targetUserId, ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isSelfUser = targetUserId == currentUser.UserId;

        var isAdmin = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        //اینجا چک میشه که کاربر فقط بتونه خودش اطلاعات مدرک تحصیلیش رو اپدیت کنه نه کس دیگه ای به جز ادمین                                                               
        if (!isAdmin && !isSelfUser)
            throw new ForbiddenException("You do not have sufficient access to manage this resume.");
    }

    private void CheckAdminPermission(ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isAdminOrEmployer = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        if (!isAdminOrEmployer)
            throw new ForbiddenException("You do not have sufficient access to manage a resume.");
    }
}
