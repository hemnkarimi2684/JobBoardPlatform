using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Dto.JobApplicationDto.Command;
using JobBoardPlatform.Application.Common.Dto.JobApplicationDto.Result;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
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

    public JobApplicationService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAdvertisementService advertisementService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _advertisementService = advertisementService;
    }

    public async Task<bool> CreateJobApplicationAsync(CreateJobApplicationCommand createCommand)
    {
        await ValidationForCreateMethod(createCommand.ResumeId, createCommand.AdvertisementId, createCommand.UserId);

        var advInformation = await _advertisementService.GetAdvertisementProjectionAsync(createCommand.AdvertisementId);

        var userFullName = await _unitOfWork.UserProfileRepository.GetUserFullNameByUserIdAsync(createCommand.UserId);

        var jobAplication = new JobApplication(JobApplicationStatus.Pending, advInformation.JobTitle, advInformation.CompanyName,
                                               advInformation.CityName, advInformation.CollaborationType, userFullName!, advInformation.ExperienceLevel,
                                               createCommand.ResumeId, createCommand.AdvertisementId, createCommand.UserId, _currentUser.UserId);

        await _unitOfWork.JobApplicationRepository.AddAsync(jobAplication);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<Pagination<JobApplicationInfoResult>> GetAdvertisementJobApplicationsAsync(Guid advertisementId, PagingCommand pagingCommand)
    {
        var userId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId);

        CheckOwnerOrAdminPermission(userId, _currentUser);

        var (advertisementJobApplications, totalDataCount) = await _unitOfWork
                                                                        .JobApplicationRepository
                                                                        .GetAdvertisementJobApplicationsAsync(ja => new JobApplicationInfoResult
                                                                        (
                                                                            ja.JobTitle,
                                                                            ja.CompanyName,
                                                                            ja.CityName,
                                                                            ja.CollaborationType,
                                                                            ja.ExperienceLevel,
                                                                            ja.Status,
                                                                            ja.CreatedAt,
                                                                            ja.UserFullName
                                                                        ),
                                                                        advertisementId,
                                                                        pagingCommand.PageNumber,
                                                                        pagingCommand.PageSize);

        return Pagination<JobApplicationInfoResult>.GetPagination(
                                                                 advertisementJobApplications,
                                                                 pagingCommand.PageNumber,
                                                                 pagingCommand.PageSize,
                                                                 totalDataCount);
    }

    public async Task<JobApplicationInfoResult> GetJobApplicationByIdAsync(Guid jobApplicationId)
    {
        var jobApplication = await _unitOfWork.JobApplicationRepository.GetByIdAsync(jobApplicationId);

        if (jobApplication == null)
            throw new NotFoundException($"the job application with id {jobApplicationId} was not found");

        var ownerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(jobApplication.AdvertisementId);

        if (ownerId == null)
            throw new NotFoundException($"Advertisement with id {jobApplication.AdvertisementId} not found.");

        CheckOwnerOrAdminOrEmployerPermission(ownerId, jobApplication.UserId, _currentUser);

        return new JobApplicationInfoResult(jobApplication.JobTitle, jobApplication.CompanyName, jobApplication.CityName,
                                            jobApplication.CollaborationType, jobApplication.ExperienceLevel, jobApplication.Status,
                                            jobApplication.CreatedAt, jobApplication.UserFullName
                                            );
    }

    public async Task<bool> UpdateJobApplicationStatusAsync(Guid jobApplicationId, string status)
    {
        var jobApplication = await _unitOfWork.JobApplicationRepository.GetByIdAsync(jobApplicationId);

        if (jobApplication == null)
            throw new NotFoundException($"the job application with id {jobApplicationId} was not found");

        var ownerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(jobApplication.AdvertisementId);

        if (ownerId == null)
            throw new NotFoundException($"Advertisement with id {jobApplication.AdvertisementId} not found.");

        CheckOwnerOrAdminPermission(ownerId, _currentUser);

        var jobApplicationStatus = ParseJobApplicationStatus(status);

        ValidateJobApplicationStatus(jobApplication, jobApplicationStatus);

        jobApplication.UpdateStatus(jobApplicationStatus, _currentUser.UserId);

        return await _unitOfWork.SaveChangesAsync() > 0;

    }

    #region Private Methods

    private async Task ValidationForCreateMethod(Guid resumeId, Guid advertisementId, Guid userId)
    {
        CheckSelfOrAdminPermission(userId, _currentUser);

        var isResumeExist = await _unitOfWork.ResumeRepository.IsResumeExistAsync(resumeId);

        if (!isResumeExist)
            throw new NotFoundException($"the resume with id {resumeId} was not found");

        var isAdvertisementExist = await _unitOfWork.AdvertisementRepository.IsAdvertisementExistAsync(advertisementId);

        if (!isAdvertisementExist)
            throw new NotFoundException($"the advertisement with id {advertisementId} was not found");

        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(userId);

        if (!isUserExist)
            throw new NotFoundException($"the user with id {userId} was not found");

        var isDuplicateJobApplication = await _unitOfWork.JobApplicationRepository.IsDuplicateJobApplicationAsync(advertisementId, userId);

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

    private void CheckOwnerOrAdminPermission(Guid? ownerId, ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isOwner = ownerId == currentUser.UserId;

        var isAdmin = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        var isEmployer = currentUser.UserRoles.Contains(RoleConstants.EmployerRoleName);

        if (!isAdmin && !(isOwner && isEmployer))
            throw new ForbiddenException("You do not have sufficient access to manage this jobApplication.");
    }

    private void CheckOwnerOrAdminOrEmployerPermission(Guid? ownerId, Guid? userId, ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isOwner = ownerId == currentUser.UserId;

        var isSelf = userId == currentUser.UserId;

        var isAdmin = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        var isEmployer = currentUser.UserRoles.Contains(RoleConstants.EmployerRoleName);

        if (!isAdmin && !(isOwner && isEmployer) && !isSelf)
            throw new ForbiddenException("You do not have sufficient access to manage this jobApplication.");
    }

    private void CheckSelfOrAdminPermission(Guid? targetUserId, ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isSelfUser = targetUserId == currentUser.UserId;

        var isAdmin = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        //اینجا چک میشه که کاربر فقط بتونه خودش اطلاعات مدرک تحصیلیش رو اپدیت کنه نه کس دیگه ای به جز ادمین                                                               
        if (!isAdmin && !isSelfUser)
            throw new ForbiddenException("You do not have sufficient access to manage this jobApplication.");
    }

    private JobApplicationStatus ParseJobApplicationStatus(string jobApplicationStatus)
    {
        if (string.IsNullOrWhiteSpace(jobApplicationStatus))
            throw new ValidationException("jobApplicationStatus is required.");

        if (!Enum.TryParse<JobApplicationStatus>(jobApplicationStatus, true, out var result))
            throw new ValidationException("Invalid jobApplicationStatus type.");

        return result;
    }

    #endregion
}
