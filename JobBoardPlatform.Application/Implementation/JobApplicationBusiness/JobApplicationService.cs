using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.Helper;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Application.Interfaces.EmailInterface;
using JobBoardPlatform.Application.Interfaces.JobApplicationInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using static JobBoardPlatform.Application.Common.AccessClaims.PermissionClaim.Permissions;

namespace JobBoardPlatform.Application.Implementation.JobApplicationBusiness;

public class JobApplicationService : IJobApplicationService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAdvertisementService _advertisementService;

    private readonly IAccessControlService _accessControlService;

    private readonly IEmailService _emailService;

    private readonly ILogger<JobApplication> _logger;

    public JobApplicationService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAdvertisementService advertisementService, IAccessControlService accessControlService, IEmailService emailService, ILogger<JobApplication> logger)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _advertisementService = advertisementService;
        _accessControlService = accessControlService;
        _emailService = emailService;
        _logger = logger;
    }

    #region Create Methods

    public async Task CreateJobApplicationAsync(
        CreateJobApplicationRequestDto createCommand,
        CancellationToken cancellationToken = default)
    {
        await ValidationForCreateMethod(createCommand.ResumeId, createCommand.AdvertisementId, createCommand.UserId, cancellationToken);

        var advInformation = await _advertisementService.GetAdvertisementProjectionAsync(createCommand.AdvertisementId, cancellationToken);

        var userFullName = await _unitOfWork.UserProfileRepository.GetUserFullNameByUserIdAsync(createCommand.UserId, cancellationToken);

        var jobApplication = new JobApplication(JobApplicationStatus.Pending, advInformation.JobTitle, advInformation.CompanyName,
                                               advInformation.CityName, advInformation.CollaborationType, userFullName!, advInformation.ExperienceLevel,
                                               createCommand.ResumeId, createCommand.AdvertisementId, createCommand.UserId, _currentUser.UserId);

        await _unitOfWork.JobApplicationRepository.AddAsync(jobApplication, cancellationToken);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!result)
            throw new ValidationException("Something went wrong while creating the job application.");

        var ownerEmail = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerEmailAsync(createCommand.AdvertisementId, cancellationToken);

        if (string.IsNullOrWhiteSpace(ownerEmail))
        {
            _logger.LogError("owner has no email");
            return;
        }

        try
        {
            await _emailService.SendAsync(ownerEmail, "New Job Application", "You have received a new job application request.", false, cancellationToken);
        }
        catch (EmailSendingException ex)
        {
            _logger.LogWarning(
                ex,
                "Job application {JobApplicationId} was created successfully, but email sending failed for advertisement {AdvertisementId}.",
                jobApplication.Id,
                createCommand.AdvertisementId);
        }
    }

    #endregion

    #region Get Methods 

    public async Task<Pagination<JobApplicationDetailResponseDto>> GetAdvertisementJobApplicationsAsync(
        Guid advertisementId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var userId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId, cancellationToken);

        _accessControlService.EnsureOwnerEmployer(userId.Value, _currentUser);

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

        _accessControlService.EnsureApplicantOrOwnerEmployer(ownerId.Value, jobApplication.UserId, _currentUser);

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
        _accessControlService.EnsureApplicant(userId, _currentUser);

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

    public List<EnumResponseDto> GetJobApplicationStatuses()
    {
        var jobApplicationStatuses = EnumHelper.GetEnumValues<JobApplicationStatus>();

        if (jobApplicationStatuses is null)
            throw new NotFoundException("there is no jobApplication status in the system.");

        return jobApplicationStatuses;
    }

    #endregion

    #region Update Methods

    public async Task UpdateJobApplicationStatusAsync(
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

        _accessControlService.EnsureOwnerEmployer(ownerId.Value, _currentUser);

        ValidateJobApplicationStatus(jobApplication, status);

        jobApplication.UpdateStatus(status, _currentUser.UserId);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!result)
            throw new ValidationException("Something went wrong while updating the job application.");

        await HandelEmailSendingForJobApplicationStatusAsync(jobApplication, status, cancellationToken);
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
        _accessControlService.EnsureApplicant(userId, _currentUser);

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

    private async Task HandelEmailSendingForJobApplicationStatusAsync(
        JobApplication jobApplication,
        JobApplicationStatus jobApplicationStatus,
        CancellationToken cancellationToken)
    {
        var userEmail = await _unitOfWork.UserRepository.GetUserEmailAsync(jobApplication.UserId, cancellationToken);

        if (string.IsNullOrWhiteSpace(userEmail))
        {
            _logger.LogError("user {UserId} has no email, jobApplicationId={JobApplicationId}", jobApplication.UserId, jobApplication.Id);
            return;
        }

        try
        {
            if (jobApplicationStatus == JobApplicationStatus.Reviewing)
                await _emailService.SendAsync(userEmail, "Job Application status is on reviewed", "Your request is being reviewed.", false, cancellationToken);

            if (jobApplicationStatus == JobApplicationStatus.Interview)
                await _emailService.SendAsync(userEmail, "Job Application status is on interview", "You have been invited to an interview.", false, cancellationToken);

            if (jobApplicationStatus == JobApplicationStatus.Rejected)
                await _emailService.SendAsync(userEmail, "Job Application status is rejected", "Your request was rejected.", false, cancellationToken);

            if (jobApplicationStatus == JobApplicationStatus.Accepted)
                await _emailService.SendAsync(userEmail, "Job Application status is accepted", "Your request has been accepted.", false, cancellationToken);
        }
        catch (EmailSendingException ex)
        {
            _logger.LogWarning(ex, "Job application {JobApplicationId} was updated successfully, but email sending failed for user {UserId}.", jobApplication.Id, jobApplication.UserId);
        }
    }

    #endregion
}
