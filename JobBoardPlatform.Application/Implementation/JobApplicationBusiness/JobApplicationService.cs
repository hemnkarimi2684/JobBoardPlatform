using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.Helper;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Application.Interfaces.EmailInterface;
using JobBoardPlatform.Application.Interfaces.JobApplicationInterface;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.EmailTemplateEntity.Constants;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
using Microsoft.Extensions.Logging;


namespace JobBoardPlatform.Application.Implementation.JobApplicationBusiness;

public class JobApplicationService : IJobApplicationService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAdvertisementService _advertisementService;

    private readonly IAccessControlService _accessControlService;

    private readonly IEmailService _emailService;

    private readonly IResumeService _resumeService;

    private readonly ILogger<JobApplication> _logger;

    public JobApplicationService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAdvertisementService advertisementService, IAccessControlService accessControlService, IEmailService emailService, IResumeService resumeService, ILogger<JobApplication> logger)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _advertisementService = advertisementService;
        _accessControlService = accessControlService;
        _emailService = emailService;
        _resumeService = resumeService;
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
            throw new ValidationException("The job application could not be created. Please try again.");

        var ownerEmail = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerEmailAsync(createCommand.AdvertisementId, cancellationToken);

        if (string.IsNullOrWhiteSpace(ownerEmail))
        {
            _logger.LogError("owner has no email");
            return;
        }

        var placeHolders = new Dictionary<string, string>
        {
              { "JobTitle", jobApplication.JobTitle }
        };

        try
        {
            await _emailService.SendTemplateEmailAsync(EmailTemplateKeys.NewJobApplicationReceived, ownerEmail, placeHolders, cancellationToken);
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

        if (userId == null)
            throw new NotFoundException("Advertisement was not found.");

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
            throw new NotFoundException("Job application was not found.");

        var ownerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(jobApplication.AdvertisementId, cancellationToken);

        if (ownerId == null)
            throw new NotFoundException("Advertisement was not found.");

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


    public async Task<ResumeDetailResponseDto> GetApplicantResumeByApplicationIdAsync(
        Guid jobApplicationId,
        Guid employerUserId,
        CancellationToken cancellationToken = default)
    {

        // اینجا چک میکنم ببینم کسی که درخواست دیدن رزومه کاربر رو کرده ایا اصلا دارای اون اگهیه یا کسی دیگه ایه که میخواد ببینه 
        var applicantUserId = await _unitOfWork.JobApplicationRepository.GetApplicantUserIdIfEmployerOwnsApplicationAsync(
            jobApplicationId,
            employerUserId,
            cancellationToken);

        if (applicantUserId is null)
            throw new NotFoundException("Job application was not found.");

        var result = await _resumeService.GetResumeDetailAsync(applicantUserId.Value, cancellationToken);

        if (result is null)
            throw new NotFoundException("The user does not have a complete resume.");

        return result;
    }


    public List<EnumResponseDto> GetJobApplicationStatuses()
    {
        var jobApplicationStatuses = EnumHelper.GetEnumValues<JobApplicationStatus>();

        if (jobApplicationStatuses is null)
            throw new NotFoundException("No job application statuses are currently available.");

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
            throw new NotFoundException("Job application was not found.");

        var ownerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(jobApplication.AdvertisementId, cancellationToken);

        if (ownerId == null)
            throw new NotFoundException("Advertisement was not found.");

        _accessControlService.EnsureOwnerEmployer(ownerId.Value, _currentUser);

        ValidateJobApplicationStatus(jobApplication, status);

        jobApplication.UpdateStatus(status, _currentUser.UserId);

        var result = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!result)
            throw new ValidationException("The job application status could not be updated.Please try again.");

        await HandelEmailSendingForJobApplicationStatusAsync(jobApplication, status, cancellationToken);
    }

    public async Task<bool> CancelJobApplicationAsync(
        Guid jobApplicationId,
        CancellationToken cancellationToken = default)
    {
        var jobApplication = await _unitOfWork.JobApplicationRepository.GetByIdAsync(jobApplicationId, cancellationToken, true);

        if (jobApplication == null)
            throw new NotFoundException("Job application was not found.");

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
            throw new NotFoundException("Resume was not found.");

        var isAdvertisementExist = await _unitOfWork.AdvertisementRepository.IsAdvertisementExistAsync(advertisementId, cancellationToken);

        if (!isAdvertisementExist)
            throw new NotFoundException("Advertisement was not found.");

        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(userId, cancellationToken);

        if (!isUserExist)
            throw new NotFoundException("User was not found.");

        var isDuplicateJobApplication = await _unitOfWork.JobApplicationRepository.IsDuplicateJobApplicationAsync(advertisementId, userId, cancellationToken);

        if (isDuplicateJobApplication)
            throw new ConflictException("You have already applied for this job.");
    }

    private void ValidateJobApplicationStatus(JobApplication jobApplication, JobApplicationStatus jobApplicationStatus)
    {
        if (jobApplicationStatus == JobApplicationStatus.Pending)
            throw new ValidationException("A job application cannot be moved back to pending status.");

        if (jobApplication.Status == JobApplicationStatus.Accepted)
            throw new ValidationException("The status of an accepted application cannot be changed.");

        if (jobApplication.Status == JobApplicationStatus.Rejected)
            throw new ValidationException("The status of a rejected application cannot be changed.");

        if (jobApplication.Status == JobApplicationStatus.Reviewing && jobApplicationStatus == JobApplicationStatus.Accepted)
            throw new ValidationException("An application under review cannot be accepted directly. An interview stage is required first.");

        if (jobApplication.Status == JobApplicationStatus.Interview && jobApplicationStatus == JobApplicationStatus.Reviewing)
            throw new ValidationException("An application cannot be moved back to the review stage after the interview stage has started.");
    }

    private async Task HandelEmailSendingForJobApplicationStatusAsync(
        JobApplication jobApplication,
        JobApplicationStatus jobApplicationStatus,
        CancellationToken cancellationToken)
    {
        var userDisplay = await _unitOfWork.UserRepository.GetUserEmailAsync(jobApplication.UserId, cancellationToken);

        if (userDisplay is null)
        {
            _logger.LogError("user {UserId} was not found, jobApplicationId={JobApplicationId}", jobApplication.UserId, jobApplication.Id);
            return;
        }

        var placeHolders = new Dictionary<string, string>()
        {
            {"CandidateName",userDisplay.FullName },
            {"JobTitle",jobApplication.JobTitle },
            {"CompanyName",jobApplication.CompanyName }
        };

        try
        {
            if (jobApplicationStatus == JobApplicationStatus.Reviewing)
                await _emailService.SendTemplateEmailAsync(EmailTemplateKeys.JobApplicationReviewing, userDisplay.Email, placeHolders, cancellationToken);

            if (jobApplicationStatus == JobApplicationStatus.Interview)
                await _emailService.SendTemplateEmailAsync(EmailTemplateKeys.JobApplicationInterview, userDisplay.Email, placeHolders, cancellationToken);

            if (jobApplicationStatus == JobApplicationStatus.Rejected)
                await _emailService.SendTemplateEmailAsync(EmailTemplateKeys.JobApplicationRejected, userDisplay.Email, placeHolders, cancellationToken);

            if (jobApplicationStatus == JobApplicationStatus.Accepted)
                await _emailService.SendTemplateEmailAsync(EmailTemplateKeys.JobApplicationAccepted, userDisplay.Email, placeHolders, cancellationToken);
        }
        catch (EmailSendingException ex)
        {
            _logger.LogWarning(ex, "Job application {JobApplicationId} was updated successfully, but email sending failed for user {UserId}.", jobApplication.Id, jobApplication.UserId);
        }
    }

    #endregion
}
