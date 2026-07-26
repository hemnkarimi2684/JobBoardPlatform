using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Application.Interfaces.JobApplicationInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;

namespace JobBoardPlatform.Application.Implementation.JobApplicationBusiness;

public class JobApplicationService : IJobApplicationService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAdvertisementService _advertisementService;

    private readonly IAccessControlService _accessControlService;

    public JobApplicationService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAdvertisementService advertisementService, IAccessControlService accessControlService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _advertisementService = advertisementService;
        _accessControlService = accessControlService;
    }

    #region Create Methods

    public async Task<bool> CreateJobApplicationAsync(
        CreateJobApplicationRequestDto createCommand,
        CancellationToken cancellationToken = default)
    {
        await ValidationForCreateMethod(createCommand.ResumeId, createCommand.AdvertisementId, createCommand.UserId, cancellationToken);

        var advInformation = await _advertisementService.GetAdvertisementProjectionAsync(createCommand.AdvertisementId, cancellationToken);

        var userFullName = await _unitOfWork.UserProfileRepository.GetUserFullNameByUserIdAsync(createCommand.UserId, cancellationToken);

        var jobAplication = new JobApplication(JobApplicationStatus.Pending, advInformation.JobTitle, advInformation.CompanyName,
                                               advInformation.CityName, advInformation.CollaborationType, userFullName!, advInformation.ExperienceLevel,
                                               createCommand.ResumeId, createCommand.AdvertisementId, createCommand.UserId, _currentUser.UserId);

        await _unitOfWork.JobApplicationRepository.AddAsync(jobAplication, cancellationToken);

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Get Methods 

    public async Task<Pagination<JobApplicationDetailResponseDto>> GetAdvertisementJobApplicationsAsync(
        Guid advertisementId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var userId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId, cancellationToken);

        _accessControlService.EnsureOwnerEmployerOrAdmin(userId.Value, _currentUser);

        var (advertisementJobApplications, totalDataCount) = await _unitOfWork
                                                                        .JobApplicationRepository
                                                                        .GetAdvertisementJobApplicationsAsync(ja => new JobApplicationDetailResponseDto
                                                                        {
                                                                            JobApplicationId = ja.Id,
                                                                            JobTitle = ja.JobTitle,
                                                                            CompanyName = ja.CompanyName,
                                                                            CityName = ja.CityName,
                                                                            CollaborationType = ja.CollaborationType,
                                                                            ExperienceLevel = ja.ExperienceLevel,
                                                                            Status = ja.Status,
                                                                            CreatedAt = ja.CreatedAt,
                                                                            UserProfileName = ja.UserFullName,
                                                                            ResumeId = ja.ResumeId,
                                                                            AdvertisementId = ja.AdvertisementId,
                                                                            UserId = ja.UserId
                                                                        },
                                                                        advertisementId,
                                                                        cancellationToken,
                                                                        pagingCommand.PageNumber,
                                                                        pagingCommand.PageSize);

        return Pagination<JobApplicationDetailResponseDto>.GetPagination(
                                                                 advertisementJobApplications,
                                                                 pagingCommand.PageNumber,
                                                                 pagingCommand.PageSize,
                                                                 totalDataCount);
    }

    public async Task<JobApplicationDetailResponseDto> GetJobApplicationByIdAsync(
        Guid jobApplicationId,
        CancellationToken cancellationToken = default)
    {
        var jobApplication = await _unitOfWork.JobApplicationRepository.GetByIdAsync(jobApplicationId, cancellationToken);

        if (jobApplication == null)
            throw new NotFoundException($"the job application with id {jobApplicationId} was not found");

        var ownerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(jobApplication.AdvertisementId, cancellationToken);

        if (ownerId == null)
            throw new NotFoundException($"Advertisement with id {jobApplication.AdvertisementId} not found.");

        _accessControlService.EnsureApplicantOrOwnerEmployerOrAdmin(ownerId.Value, jobApplication.UserId, _currentUser);

        return new JobApplicationDetailResponseDto
        {
            JobApplicationId = jobApplication.Id,
            JobTitle = jobApplication.JobTitle,
            CompanyName = jobApplication.CompanyName,
            CityName = jobApplication.CityName,
            CollaborationType = jobApplication.CollaborationType,
            ExperienceLevel = jobApplication.ExperienceLevel,
            Status = jobApplication.Status,
            CreatedAt = jobApplication.CreatedAt,
            UserProfileName = jobApplication.UserFullName,
            ResumeId = jobApplication.ResumeId,
            AdvertisementId = jobApplication.AdvertisementId,
            UserId = jobApplication.UserId
        };
    }

    public async Task<Pagination<JobApplicationDetailResponseDto>> GetJobApplicationsByUserIdAsync(
        Guid userId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        var (advertisementJobApplications, totalDataCount) = await _unitOfWork
                                                                        .JobApplicationRepository
                                                                        .GetJobApplicationsByUserIdAsync(ja => new JobApplicationDetailResponseDto
                                                                        {
                                                                            JobApplicationId = ja.Id,
                                                                            JobTitle = ja.JobTitle,
                                                                            CompanyName = ja.CompanyName,
                                                                            CityName = ja.CityName,
                                                                            CollaborationType = ja.CollaborationType,
                                                                            ExperienceLevel = ja.ExperienceLevel,
                                                                            Status = ja.Status,
                                                                            CreatedAt = ja.CreatedAt,
                                                                            UserProfileName = ja.UserFullName,
                                                                            ResumeId = ja.ResumeId,
                                                                            AdvertisementId = ja.AdvertisementId,
                                                                            UserId = ja.UserId
                                                                        },
                                                                        userId,
                                                                        cancellationToken,
                                                                        pagingCommand.PageNumber,
                                                                        pagingCommand.PageSize);

        return Pagination<JobApplicationDetailResponseDto>.GetPagination(
                                                                 advertisementJobApplications,
                                                                 pagingCommand.PageNumber,
                                                                 pagingCommand.PageSize,
                                                                 totalDataCount);
    }


    #endregion

    #region Update Methods

    public async Task<bool> UpdateJobApplicationStatusAsync(
        Guid jobApplicationId,
        JobApplicationStatus status,
        CancellationToken cancellationToken = default)
    {
        var jobApplication = await _unitOfWork.JobApplicationRepository.GetByIdAsync(jobApplicationId, cancellationToken, true);

        if (jobApplication == null)
            throw new NotFoundException($"the job application with id {jobApplicationId} was not found");

        var ownerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(jobApplication.AdvertisementId, cancellationToken);

        if (ownerId == null)
            throw new NotFoundException($"Advertisement with id {jobApplication.AdvertisementId} not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(ownerId.Value, _currentUser);

        ValidateJobApplicationStatus(jobApplication, status);

        jobApplication.UpdateStatus(status, _currentUser.UserId);

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

    }

    public async Task<bool> CancelJobApplicationAsync(
        Guid jobApplicationId,
        CancellationToken cancellationToken = default)
    {
        var jobApplication = await _unitOfWork.JobApplicationRepository.GetByIdAsync(jobApplicationId, cancellationToken, true);

        if (jobApplication == null)
            throw new NotFoundException($"the job application with id {jobApplicationId} was not found.");

        _accessControlService.EnsureApplicant(jobApplication.UserId, _currentUser);

        jobApplication.Cancel(_currentUser.UserId);

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Private Methods

    private async Task ValidationForCreateMethod(Guid resumeId, Guid advertisementId, Guid userId, CancellationToken cancellationToken)
    {
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        var isResumeExist = await _unitOfWork.ResumeRepository.IsResumeExistAsync(resumeId, cancellationToken);

        if (!isResumeExist)
            throw new NotFoundException($"the resume with id {resumeId} was not found");

        var isAdvertisementExist = await _unitOfWork.AdvertisementRepository.IsAdvertisementExistAsync(advertisementId, cancellationToken);

        if (!isAdvertisementExist)
            throw new NotFoundException($"the advertisement with id {advertisementId} was not found");

        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(userId, cancellationToken);

        if (!isUserExist)
            throw new NotFoundException($"the user with id {userId} was not found");

        var isDuplicateJobApplication = await _unitOfWork.JobApplicationRepository.IsDuplicateJobApplicationAsync(advertisementId, userId, cancellationToken);

        if (isDuplicateJobApplication)
            throw new ConflictException($" the user with id {userId} for advertisement with id {advertisementId} already has jobApplication");
    }

    private void ValidateJobApplicationStatus(JobApplication jobApplication, JobApplicationStatus jobApplicationStatus)
    {
        if (jobApplicationStatus == JobApplicationStatus.Pending)
            throw new ValidationException("the jobApplication cannot return to a pending status");

        if (jobApplication.Status == JobApplicationStatus.Accepted)
            throw new ValidationException("You cannot change the Accepted status.");

        if (jobApplication.Status == JobApplicationStatus.Rejected)
            throw new ValidationException("You cannot change the rejected status.");

        if (jobApplication.Status == JobApplicationStatus.Reviewing)
        {
            if (jobApplicationStatus == JobApplicationStatus.Accepted)
                throw new ValidationException("Applications currently under review cannot be accepted directly without an interview stage.");
        }

        if (jobApplication.Status == JobApplicationStatus.Interview)
        {
            if (jobApplicationStatus == JobApplicationStatus.Reviewing)
                throw new ValidationException("The status cannot be reverted from Interviewing to Under Review, as the review has already been completed.");
        }
    }

    #endregion
}
