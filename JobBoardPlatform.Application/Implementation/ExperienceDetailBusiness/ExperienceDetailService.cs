using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Dto.ExperienceDetailDto.Command;
using JobBoardPlatform.Application.Common.Dto.ExperienceDetailDto.Result;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;

namespace JobBoardPlatform.Application.Implementation.ExperienceDetailBusiness;

public class ExperienceDetailService : IExperienceDetailService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    public ExperienceDetailService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<bool> CreateExperienceDetailAsync(CreateExperienceDetailCommand createCommand)
    {
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(createCommand.UserId);

        if (!isUserExist)
            throw new NotFoundException($"The user with id {createCommand.UserId} was not found.");

        CheckSelfOrAdminPermission(createCommand.UserId, _currentUser);

        var seniorityLevel = ParseSeniorityLevelForCreate(createCommand.SeniorityLevel);

        var experienceDetail = new ExperienceDetail(
                                                    createCommand.LastJobTitle,
                                                    seniorityLevel,
                                                    createCommand.JobCategory,
                                                    createCommand.City,
                                                    createCommand.StartDate,
                                                    createCommand.EndDate,
                                                    createCommand.IsCurrentJob,
                                                    createCommand.UserId,
                                                    _currentUser.UserId);

        await _unitOfWork.ExperienceDetailRepository.AddAsync(experienceDetail);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<Pagination<UserExperienceDetailResult>> GetUserExperienceDetailsAsync(Guid userId, PagingCommand pagingCommand)
    {
        CheckSelfOrAdminPermission(userId, _currentUser);

        var (experienceDetails, totalDataCount) = await _unitOfWork
                                                                 .ExperienceDetailRepository
                                                                 .GetUserExperienceDetailsAsync(ed => new UserExperienceDetailResult
                                                                 (
                                                                     ed.LastJobTitle,
                                                                     ed.SeniorityLevel,
                                                                     ed.JobCategory,
                                                                     ed.City,
                                                                     ed.StartDate,
                                                                     ed.EndDate,
                                                                     ed.IsCurrentJob
                                                                 ),
                                                                  userId,
                                                                  pagingCommand.PageNumber,
                                                                  pagingCommand.PageSize);

        return Pagination<UserExperienceDetailResult>
                                            .GetPagination(
                                                            experienceDetails,
                                                            pagingCommand.PageNumber,
                                                            pagingCommand.PageSize,
                                                            totalDataCount);
    }

    public async Task<bool> UpdateExperienceDetailAsync(Guid experienceDetailId, UpdateExperienceDetailCommand updateCommand)
    {
        var userId = await _unitOfWork.ExperienceDetailRepository.GetExperienceDetailUserIdAsync(experienceDetailId);

        if (userId == null)
            throw new NotFoundException($"The experience detail with id {experienceDetailId} was not found.");

        CheckSelfOrAdminPermission(userId, _currentUser);

        var result = await _unitOfWork.ExperienceDetailRepository.UpdateExperienceDetailAsync(
                                                                                              experienceDetailId,
                                                                                              MapToUpdateExperienceDetail(updateCommand));

        if (!result)
            throw new NotFoundException($"the experienceDetail with id {experienceDetailId} was not found");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #region Private Methods

    private SeniorityLevel ParseSeniorityLevelForCreate(string seniorityLevel)
    {
        if (string.IsNullOrWhiteSpace(seniorityLevel))
            throw new ValidationException("seniorityLevel is required.");

        if (!Enum.TryParse<SeniorityLevel>(seniorityLevel, true, out var result))
            throw new ValidationException("Invalid seniorityLevel type.");

        return result;
    }

    private SeniorityLevel? ParseSeniorityLevelForUpdate(string? seniorityLevel)
    {
        if (string.IsNullOrWhiteSpace(seniorityLevel))
            return null;

        if (!Enum.TryParse<SeniorityLevel>(seniorityLevel, true, out var result))
            throw new ValidationException("Invalid seniorityLevel type.");

        return result;
    }

    private UpdateExperienceDetail MapToUpdateExperienceDetail(UpdateExperienceDetailCommand updateCommand)
    {
        var seniorityLevel = ParseSeniorityLevelForUpdate(updateCommand.SeniorityLevel);

        return new UpdateExperienceDetail
        (
           updateCommand.LastJobTitle,
           seniorityLevel,
           updateCommand.JobCategory,
           updateCommand.City,
           updateCommand.StartDate,
           updateCommand.EndDate,
           updateCommand.IsCurrentJob,
           _currentUser.UserId
        );
    }

    private void CheckSelfOrAdminPermission(Guid? targetUserId, ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isSelfUser = targetUserId == currentUser.UserId;

        var isAdmin = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        //اینجا چک میشه که کاربر فقط بتونه خودش اطلاعات تجربه کاریش رو اپدیت کنه نه کس دیگه ای به جز ادمین                                                               
        if (!isAdmin && !isSelfUser)
            throw new ForbiddenException("You do not have sufficient access to manage this ExperienceDetail.");
    }

    #endregion
}
