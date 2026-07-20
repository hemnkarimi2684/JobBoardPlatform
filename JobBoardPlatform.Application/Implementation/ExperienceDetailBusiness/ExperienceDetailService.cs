using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.ExperienceDetailBusiness;

public class ExperienceDetailService : IExperienceDetailService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    public ExperienceDetailService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
    }

    public async Task<bool> CreateExperienceDetailAsync(CreateExperienceDetailRequestDto createCommand)
    {
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(createCommand.UserId);

        if (!isUserExist)
            throw new NotFoundException($"The user with id {createCommand.UserId} was not found.");

        _accessControlService.EnsureApplicantOrAdmin(createCommand.UserId, _currentUser);

        var experienceDetail = new ExperienceDetail(
                                                    createCommand.LastJobTitle,
                                                    createCommand.SeniorityLevel,
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

    public async Task<Pagination<UserExperienceDetailResponseDto>> GetUserExperienceDetailsAsync(Guid userId, PagingRequestDto pagingCommand)
    {
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        var (experienceDetails, totalDataCount) = await _unitOfWork
                                                                 .ExperienceDetailRepository
                                                                 .GetUserExperienceDetailsAsync(ed => new UserExperienceDetailResponseDto
                                                                 (
                                                                     ed.Id,
                                                                     ed.LastJobTitle,
                                                                     ed.UserId,
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

        return Pagination<UserExperienceDetailResponseDto>
                                            .GetPagination(
                                                            experienceDetails,
                                                            pagingCommand.PageNumber,
                                                            pagingCommand.PageSize,
                                                            totalDataCount);
    }

    public async Task<bool> UpdateExperienceDetailAsync(Guid experienceDetailId, UpdateExperienceDetailRequestDto updateCommand)
    {
        var userId = await _unitOfWork.ExperienceDetailRepository.GetExperienceDetailUserIdAsync(experienceDetailId);

        if (userId == null)
            throw new NotFoundException($"The experience detail with id {experienceDetailId} was not found.");

        _accessControlService.EnsureApplicantOrAdmin(userId.Value, _currentUser);

        var result = await _unitOfWork.ExperienceDetailRepository.UpdateExperienceDetailAsync(
                                                                                              experienceDetailId,
                                                                                              MapToUpdateExperienceDetail(updateCommand));

        if (!result)
            throw new NotFoundException($"the experienceDetail with id {experienceDetailId} was not found");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #region Private Methods

    private UpdateExperienceDetail MapToUpdateExperienceDetail(UpdateExperienceDetailRequestDto updateCommand)
    {
        return new UpdateExperienceDetail
        (
           updateCommand.LastJobTitle,
           updateCommand.SeniorityLevel,
           updateCommand.JobCategory,
           updateCommand.City,
           updateCommand.StartDate,
           updateCommand.EndDate,
           updateCommand.IsCurrentJob,
           _currentUser.UserId
        );
    }

    #endregion
}
