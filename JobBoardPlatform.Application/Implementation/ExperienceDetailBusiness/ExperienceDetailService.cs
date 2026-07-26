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

    #region Create Methods

    public async Task<bool> CreateExperienceDetailAsync(
        CreateExperienceDetailRequestDto createCommand,
        CancellationToken cancellationToken = default)
    {
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(createCommand.UserId, cancellationToken);

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

        await _unitOfWork.ExperienceDetailRepository.AddAsync(experienceDetail, cancellationToken);

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Get Methods

    public async Task<Pagination<ExperienceHistoryResponseDto>> GetUserExperienceDetailsAsync(
        Guid userId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        var (experienceDetails, totalDataCount) = await _unitOfWork
                                                                 .ExperienceDetailRepository
                                                                 .GetUserExperienceDetailsAsync(ed => new ExperienceHistoryResponseDto
                                                                 {
                                                                     ExperienceDetailId = ed.Id,
                                                                     LastJobTitle = ed.LastJobTitle,
                                                                     UserId = ed.UserId,
                                                                     SeniorityLevel = ed.SeniorityLevel,
                                                                     JobCategory = ed.JobCategory,
                                                                     City = ed.City,
                                                                     StartDate = ed.StartDate,
                                                                     EndDate = ed.EndDate,
                                                                     IsCurrentJob = ed.IsCurrentJob
                                                                 },
                                                                  userId,
                                                                  cancellationToken,
                                                                  pagingCommand.PageNumber,
                                                                  pagingCommand.PageSize);

        return Pagination<ExperienceHistoryResponseDto>
                                            .GetPagination(
                                                            experienceDetails,
                                                            pagingCommand.PageNumber,
                                                            pagingCommand.PageSize,
                                                            totalDataCount);
    }


    public async Task<ExperienceHistoryResponseDto> GetExperienceDetailByIdAsync(Guid experienceDetailId, CancellationToken cancellationToken = default)
    {
        var experienceDetail = await _unitOfWork.ExperienceDetailRepository.GetByIdAsync(experienceDetailId, cancellationToken);

        if (experienceDetail == null)
            throw new NotFoundException($"The experience detail with id {experienceDetailId} was not found.");

        _accessControlService.EnsureApplicantOrAdmin(experienceDetail.UserId, _currentUser);

        return new ExperienceHistoryResponseDto
        {
            ExperienceDetailId = experienceDetail.Id,
            LastJobTitle = experienceDetail.LastJobTitle,
            UserId = experienceDetail.UserId,
            SeniorityLevel = experienceDetail.SeniorityLevel,
            JobCategory = experienceDetail.JobCategory,
            City = experienceDetail.City,
            StartDate = experienceDetail.StartDate,
            EndDate = experienceDetail.EndDate,
            IsCurrentJob = experienceDetail.IsCurrentJob
        };
    }


    #endregion

    #region Update Methods

    public async Task<bool> UpdateExperienceDetailAsync(
        Guid experienceDetailId,
        UpdateExperienceDetailRequestDto updateCommand,
        CancellationToken cancellationToken = default)
    {
        var userId = await _unitOfWork.ExperienceDetailRepository.GetExperienceDetailUserIdAsync(experienceDetailId, cancellationToken);

        if (userId == null)
            throw new NotFoundException($"The experience detail with id {experienceDetailId} was not found.");

        _accessControlService.EnsureApplicantOrAdmin(userId.Value, _currentUser);

        var result = await _unitOfWork.ExperienceDetailRepository.UpdateExperienceDetailAsync(
                                                                                              experienceDetailId,
                                                                                              cancellationToken,
                                                                                              MapToUpdateExperienceDetail(updateCommand));

        if (!result)
            throw new NotFoundException($"the experienceDetail with id {experienceDetailId} was not found");

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Private Methods

    private UpdateExperienceDetail MapToUpdateExperienceDetail(UpdateExperienceDetailRequestDto updateCommand)
    {
        return new UpdateExperienceDetail
        {
            LastJobTitle = updateCommand.LastJobTitle,
            SeniorityLevel = updateCommand.SeniorityLevel,
            JobCategory = updateCommand.JobCategory,
            City = updateCommand.City,
            StartDate = updateCommand.StartDate,
            EndDate = updateCommand.EndDate,
            IsCurrentJob = updateCommand.IsCurrentJob,
            ModifiedById = _currentUser.UserId
        };
    }

    #endregion
}
