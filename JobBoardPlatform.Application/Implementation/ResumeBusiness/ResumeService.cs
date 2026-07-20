using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AttachmentInterface;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JobBoardPlatform.Application.Implementation.ResumeBusiness;


public class ResumeService : IResumeService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAttachmentService _attachmentService;

    private readonly IAccessControlService _accessControlService;

    private readonly ILogger<ResumeService> _logger;

    public ResumeService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAttachmentService attachmentService, IAccessControlService accessControlService, ILogger<ResumeService> logger)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
        _logger = logger;
    }

    #region Create Methods

    public async Task<bool> CreateResumeAsync(CreateResumeRequestDto resumeCommand)
    {
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(resumeCommand.UserId);

        if (!isUserExist)
            throw new NotFoundException($"the user with id {resumeCommand.UserId} was not found");

        var isUserHasProfile = await _unitOfWork.UserProfileRepository.IsUserHasProfileAsync(resumeCommand.UserId);

        if (isUserHasProfile)
            throw new NotFoundException($"The user with id '{resumeCommand.UserId}' does not have a complete profile.");

        _accessControlService.EnsureApplicant(resumeCommand.UserId, _currentUser);

        var isDuplicateResumeFortUser = await _unitOfWork.ResumeRepository.IsDuplicateResumeForUserAsync(resumeCommand.UserId);

        if (isDuplicateResumeFortUser)
            throw new ConflictException($"the user with id {resumeCommand.UserId} already has resume");

        var resume = new Resume(resumeCommand.Title, resumeCommand.UserId, null, _currentUser.UserId);

        await _unitOfWork.ResumeRepository.AddAsync(resume);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Delete Methods

    public async Task<bool> DeleteResumeFileByIdAsync(Guid resumeId)
    {
        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(resumeId);

        if (resume == null)
            throw new NotFoundException($"The resume with id {resumeId} was not found.");

        _accessControlService.EnsureApplicant(resume.UserId, _currentUser);

        if (resume.LastUploadedFileId == null)
            throw new ValidationException("This resume does not have any file to delete.");

        //اینجا اول میام اپدیت رو انجام میدم بعد سیو چینج میزنم بلافاصله 
        //چرا؟ چون اگه حذف کردن رو هم توی سیو چینج میزاشتم ممکن بود فایل حذف بشه اما زمان اپدیت به مشکل بخوره و الان اگه رول بکی بخواد انجام بشه 
        //فایلی وجود ندارد توی دیتابیس و حذف شده پس اول اپدیت انجام میدم اگه مشکلی نداشت اون رو حذف میکنم 
        var attachmentId = resume.LastUploadedFileId.Value;

        resume.UpdateFile(null);

        await _unitOfWork.SaveChangesAsync();

        var deleted = await _attachmentService.HardDeleteAttachmentAsync(attachmentId);

        if (!deleted)
            throw new ValidationException("Resume file reference removed, but deleting the attachment failed.");

        return deleted;
    }

    #endregion

    #region Get Methods

    public async Task<ResumeDetailResponseDto> GetResumeDetailAsync(Guid userId)
    {
        var resumeId = await _unitOfWork.ResumeRepository.GetResumeIdByUserIdAsync(userId);

        if (resumeId == null)
            throw new NotFoundException($"the resume for user with id {userId} not found");

        await EnsureUserCanAccessResumeAsync(resumeId.Value, userId, _currentUser);

        var result = await _unitOfWork.UserRepository.GetResumeDetailAsync(u => new ResumeDetailResponseDto
        {
            Title = u.Resume.Title == null ? null : u.Resume.Title,
            ResumeId = u.Resume.Id == null ? null : u.Resume.Id,
            UserId = u.Id,
            ResumeFileId = u.Resume.LastUploadedFileId == null ? null : u.Resume.LastUploadedFileId,

            ResumeUserProfiles = u.UserProfile != null ? new ResumeUserProfileResponseDto(
                     u.UserProfile.FirstName + " " + u.UserProfile.LastName,
                     u.UserProfile.Bio,
                     u.UserProfile.Address,
                     u.UserProfile.BirthDate,
                     u.UserProfile.City.Name,
                     u.UserProfile.Gender
                     ) : null,

            ResumeEducationDetails = u.EducationDetails.Select(ed => new ResumeEducationDetailResponseDto(
                ed.Id,
                ed.CertificateDegreeName,
                ed.Major,
                ed.University,
                ed.StartDate,
                ed.CompletionDate,
                ed.Percentage,
                ed.IsCurrentlyStudying)).ToList(),

            ResumeExperienceDetails = u.ExperienceDetails.Select(ed => new ResumeExperienceDetailResponseDto(
                ed.Id,
                ed.LastJobTitle,
                ed.SeniorityLevel,
                ed.JobCategory,
                ed.City,
                ed.StartDate,
                ed.EndDate,
                ed.IsCurrentJob
                )).ToList(),

            ResumeSkills = u.UserSkills.Select(us => new ResumeSkillDetailResponseDto(
                us.SkillId,
                us.Skill.Name
                )).ToList()
        },
          userId);

        if (result is null)
            throw new NotFoundException($"The user with id '{userId}' does not have a complete resume/profile.");

        return result!;

    }

    #endregion

    #region Upload Resume File Methods

    public async Task UploadResumeFileByResumeIdAsync(Guid resumeId, UploadResumeFileRequestDto uploadResumeFile)
    {
        if (uploadResumeFile?.File is null)
            throw new ValidationException("Resume file is required.");

        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(resumeId);

        if (resume == null)
            throw new NotFoundException($"The resume with id {resumeId} was not found.");

        _accessControlService.EnsureApplicant(resume.UserId, _currentUser);

        await UploadResumeFileAsync(resume, uploadResumeFile.File);
    }

    public async Task UploadResumeFileByUserIdAsync(Guid userId, UploadResumeFileRequestDto uploadResumeFile)
    {
        if (uploadResumeFile?.File is null)
            throw new ValidationException("Resume file is required.");

        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(userId);

        if (!isUserExist)
            throw new NotFoundException($"the user with id {userId} was not found");

        var resume = await _unitOfWork.ResumeRepository.GetResumeByUserIdAsync(userId);

        if (resume == null)
            throw new NotFoundException($"the user with id {userId} dont have any resume.");

        _accessControlService.EnsureApplicant(resume.UserId, _currentUser);

        await UploadResumeFileAsync(resume, uploadResumeFile.File);
    }

    #endregion

    #region Download Resume File Methods

    public async Task<AttachmentResponseDto> DownloadResumeFileByResumeIdAsync(Guid resumeId)
    {
        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(resumeId);

        if (resume == null)
            throw new NotFoundException($"The resume with id '{resumeId}' was not found.");

        if (resume.LastUploadedFileId == null)
            throw new NotFoundException($"The resume with id '{resume.Id}' does not have an attached file.");

        await EnsureUserCanAccessResumeAsync(resume.Id, resume.UserId, _currentUser);

        return await _attachmentService.DownloadAsync(resume.LastUploadedFileId.Value);
    }

    public async Task<AttachmentResponseDto> DownloadResumeFileByUserIdAsync(Guid userId)
    {
        var resume = await _unitOfWork.ResumeRepository.GetResumeByUserIdAsync(userId);

        if (resume == null)
            throw new NotFoundException($"The user with id '{userId}' does not have a resume.");

        if (resume.LastUploadedFileId == null)
            throw new NotFoundException($"The user with id '{userId}' does not have an attached file.");

        await EnsureUserCanAccessResumeAsync(resume.Id, resume.UserId, _currentUser);

        return await _attachmentService.DownloadAsync(resume.LastUploadedFileId.Value);
    }

    #endregion

    #region Private Methods

    private async Task EnsureUserCanAccessResumeAsync(Guid resumeId, Guid? targetUserId, ICurrentUser currentUser)
    {
        // چک کردن اینکه اول ایای خودش داره درخواست میده یا نه
        var isSelfUser = targetUserId == currentUser.UserId;

        //چک کردن اینکه ایای ادمین دارهد درخواست میده یا نه
        var isAdmin = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        // ایا کارفرماس که داره درخواست میده 
        var isEmployer = currentUser.UserRoles.Contains(RoleConstants.EmployerRoleName);

        if (isAdmin)
            return;

        if (isSelfUser)
            return;

        if (isEmployer)
        {
            ///اینجا دارم چک میکنم که اگر کارفرما بود که داشت درخواست میداد 
            ///باید چک شه ایا این درخواستی که برای دیدن رزومه میده اصلا این رزومه برای کسی که درخواست رو برای اگهیش فرستاده یا نه 
            var hasJobApplication = await _unitOfWork.JobApplicationRepository
                                                        .CheckOwnerHasJobApplicationForResumeAsync(resumeId, currentUser.UserId);
            ///حالا اگه برای اون درخواست بود ریترن میکنه
            if (hasJobApplication)
                return;
        }

        throw new ForbiddenException("You do not have sufficient access to view this resume.");
    }

    private async Task DeleteAttachmentAsync(Guid attachmentId)
    {
        try
        {
            await _attachmentService.HardDeleteAttachmentAsync(attachmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete attachment {AttachmentId}", attachmentId);
        }
    }

    private async Task UploadResumeFileAsync(Resume resume, IFormFile file)
    {
        //نگه داشتن ایدی قبلی فایل اپلود شده 
        var oldFileId = resume.LastUploadedFileId;
        Guid? newFileId = null;

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            newFileId = await _attachmentService.UploadAsync(file, AttachmentType.Document);

            resume.UpdateFile(newFileId);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync();

            //اینجا برای این ترای کچ کذاشتم که اگه توی فلو اضافه کردن و اپدیت کردن فایل به رزومه به اکسپشن و مشکلی خورد....
            //و فایل جدیدی اپلود شده بود اما بدون اینکه به شرکت اختصاص داشته باشه اینو بیام حذف کنم 
            if (newFileId != null)
                await DeleteAttachmentAsync(newFileId.Value);

            throw;
        }

        //حالا اگه فایل رزومه جدیدی سیو شد و اپدیت شد بیا اون فایل رزومه قدیمی رو حذف کن 
        if (oldFileId != null)
            await DeleteAttachmentAsync(oldFileId.Value);
    }

    #endregion
}
