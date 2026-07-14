using JobBoardPlatform.Application.Common.Constants.Authentication;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Dto.EducationDetailDto.Command;
using JobBoardPlatform.Application.Common.Dto.EducationDetailDto.Result;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.EducationDetailInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.EducationDetailBusiness;

public class EducationDetailService : IEducationDetailService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    public EducationDetailService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<bool> CreateEducationDetailAsync(CreateEducationDetailCommand createCommand)
    {
        CheckSelfOrAdminPermission(createCommand.UserId, _currentUser);

        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(createCommand.UserId);

        if (!isUserExist)
            throw new NotFoundException($"user with id {createCommand.UserId} was not found");

        var certificateDegreeName = ParseEnums(createCommand.CertificateDegreeName);

        var educationDetail = new EducationDetail(
                                           certificateDegreeName,
                                           createCommand.Major,
                                           createCommand.University,
                                           createCommand.StartDate,
                                           createCommand.CompletionDate,
                                           createCommand.Percentage,
                                           createCommand.IsCurrentlyStudying,
                                           createCommand.UserId,
                                           _currentUser.UserId
                                           );

        await _unitOfWork.EducationDetailRepository.AddAsync(educationDetail);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<Pagination<UserEducationDetailResult>> GetUserEducationDetailsAsync(Guid userId, PagingCommand pagingCommand)
    {
        CheckSelfOrAdminPermission(userId, _currentUser);

        var (userEducationDetails, totalDataCount) = await _unitOfWork.EducationDetailRepository
                                                          .GetUserEducationDetailsAsync(ed =>
                                                          new UserEducationDetailResult
                                                          (
                                                              ed.CertificateDegreeName,
                                                              ed.Major,
                                                              ed.University,
                                                              ed.StartDate,
                                                              ed.CompletionDate,
                                                              ed.Percentage,
                                                              ed.IsCurrentlyStudying
                                                          ),
                                                          userId,
                                                          pagingCommand.PageNumber,
                                                          pagingCommand.PageSize);

        return Pagination<UserEducationDetailResult>
                    .GetPagination(userEducationDetails,
                                   pagingCommand.PageNumber,
                                   pagingCommand.PageSize,
                                   totalDataCount);
    }

    public async Task<bool> UpdateEducationDetailAsync(Guid educationDetailId, UpdateEducationDetailCommand updateCommand)
    {
        var userId = await _unitOfWork.EducationDetailRepository.GetEducationDetailUserIdAsync(educationDetailId);

        if (userId == null)
            throw new NotFoundException($"The education detail with id {educationDetailId} was not found.");

        CheckSelfOrAdminPermission(userId, _currentUser);

        var result = await _unitOfWork.EducationDetailRepository.UpdateEducationDetailAsync(
                                                                                            educationDetailId,
                                                                                            MapToUpdateEducationDetail(updateCommand));

        if (!result)
            throw new NotFoundException($"the educationDetail with id {educationDetailId} was not found");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #region Private Methods

    private CertificateDegree ParseEnums(string? certificateDegree)
    {
        if (string.IsNullOrWhiteSpace(certificateDegree))
            throw new ValidationException("certificateDegree is required.");

        if (!Enum.TryParse<CertificateDegree>(certificateDegree, true, out var result))
            throw new ValidationException("Invalid certificateDegree type.");

        return result;
    }

    private UpdateEducationDetail MapToUpdateEducationDetail(UpdateEducationDetailCommand updateCommand)
    {
        var parsedEnum = ParseEnums(updateCommand.CertificateDegreeName);

        return new UpdateEducationDetail
        (
           parsedEnum,
           updateCommand.Major,
           updateCommand.University,
           updateCommand.StartDate,
           updateCommand.CompletionDate,
           updateCommand.Percentage,
           updateCommand.IsCurrentlyStudying,
           _currentUser.UserId
        );
    }

    private void CheckSelfOrAdminPermission(Guid? targetUserId, ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isSelfUser = targetUserId == currentUser.UserId;

        var isAdmin = currentUser.UserRoles.Any(role => role == RoleConstants.AdminRoleName);

        //اینجا چک میشه که کاربر فقط بتونه خودش اطلاعات مدرک تحصیلیش رو اپدیت کنه نه کس دیگه ای به جز ادمین                                                               
        if (!isAdmin && !isSelfUser)
            throw new ForbiddenException("You do not have sufficient access to manage this advertisement.");
    }

    #endregion
}
