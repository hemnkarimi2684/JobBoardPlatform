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
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
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

    public async Task<bool> CreateResumeAsync(
        CreateResumeRequestDto resumeCommand,
        CancellationToken cancellationToken = default)
    {
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(resumeCommand.UserId, cancellationToken);

        if (!isUserExist)
            throw new NotFoundException($"the user with id {resumeCommand.UserId} was not found");

        var isUserHasProfile = await _unitOfWork.UserProfileRepository.IsUserHasProfileAsync(resumeCommand.UserId, cancellationToken);

        if (!isUserHasProfile)
            throw new NotFoundException($"The user with id '{resumeCommand.UserId}' does not have a complete profile.");

        _accessControlService.EnsureApplicant(resumeCommand.UserId, _currentUser);

        var isDuplicateResumeFortUser = await _unitOfWork.ResumeRepository.IsDuplicateResumeForUserAsync(resumeCommand.UserId, cancellationToken);

        if (isDuplicateResumeFortUser)
            throw new ConflictException($"the user with id {resumeCommand.UserId} already has resume");

        var resume = new Resume(resumeCommand.Title, resumeCommand.UserId, null, _currentUser.UserId);

        await _unitOfWork.ResumeRepository.AddAsync(resume, cancellationToken);

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Delete Methods

    public async Task DeleteResumeFileByIdAsync(
        Guid resumeId,
        CancellationToken cancellationToken = default)
    {
        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(resumeId, cancellationToken, true);

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

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            //ذخیره تغییر رزومه و نال کردن پرارپتی ایدی اتچمنتش 
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // حذف سخت فایل و رکورد اتچمنت از دیتابیس
            var deleted = await _attachmentService.HardDeleteAttachmentAsync(attachmentId, cancellationToken);

            if (!deleted)
                throw new InvalidOperationException("Failed to delete the attachment file or database record.");

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            _logger.LogError(ex, "Failed to delete resume file for resumeId: {ResumeId}, AttachmentId: {AttachmentId}. Transaction rolled back.",
                resumeId, attachmentId);

            throw;
        }
    }

    #endregion

    #region Get Methods

    public async Task<ResumeDetailResponseDto> GetResumeDetailAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var resumeId = await _unitOfWork.ResumeRepository.GetResumeIdByUserIdAsync(userId, cancellationToken);

        if (resumeId == null)
            throw new NotFoundException($"the resume for user with id {userId} not found");

        await EnsureUserCanAccessResumeAsync(resumeId.Value, userId, _currentUser, cancellationToken);

        var result = await _unitOfWork.UserRepository.GetResumeDetailAsync(u => new ResumeDetailResponseDto
        {
            Title = u.Resume.Title == null ? null : u.Resume.Title,
            ResumeId = u.Resume.Id == null ? null : u.Resume.Id,
            UserId = u.Id,
            ResumeFileId = u.Resume.LastUploadedFileId == null ? null : u.Resume.LastUploadedFileId,

            ResumeUserProfiles = u.UserProfile != null ? new ResumeUserProfileResponseDto
            {
                FullName = u.UserProfile.FirstName + " " + u.UserProfile.LastName,
                Bio = u.UserProfile.Bio,
                Address = u.UserProfile.Address,
                BirthDate = u.UserProfile.BirthDate,
                CityName = u.UserProfile.City.Name,
                Gender = u.UserProfile.Gender,
                UserImageFileId = u.UserProfile.UserImageFileId
            } : null,

            ResumeEducationDetails = u.EducationDetails.Select(ed => new ResumeEducationDetailResponseDto
            {
                EducationDetailId = ed.Id,
                CertificateDegreeName = ed.CertificateDegreeName,
                Major = ed.Major,
                University = ed.University,
                StartDate = ed.StartDate,
                CompletionDate = ed.CompletionDate,
                Percentage = ed.Percentage,
                IsCurrentlyStudying = ed.IsCurrentlyStudying
            }).ToList(),

            ResumeExperienceDetails = u.ExperienceDetails.Select(ed => new ResumeExperienceDetailResponseDto
            {
                ExperienceDetailId = ed.Id,
                LastJobTitle = ed.LastJobTitle,
                SeniorityLevel = ed.SeniorityLevel,
                JobCategory = ed.JobCategory,
                City = ed.City,
                StartDate = ed.StartDate,
                EndDate = ed.EndDate,
                IsCurrentJob = ed.IsCurrentJob
            }).ToList(),

            ResumeSkills = u.UserSkills.Select(us => new ResumeSkillDetailResponseDto
            {
                SkillId = us.SkillId,
                SkillName = us.Skill.Name
            }).ToList()
        },
          userId, cancellationToken);

        if (result is null)
            throw new NotFoundException($"The user with id '{userId}' does not have a complete resume/profile.");

        return result!;

    }

    #endregion

    #region Upload Resume File Methods

    public async Task UploadResumeFileByResumeIdAsync(
        Guid resumeId,
        UploadResumeFileRequestDto uploadResumeFile,
        CancellationToken cancellationToken = default)
    {
        if (uploadResumeFile?.File is null)
            throw new ValidationException("Resume file is required.");

        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(resumeId, cancellationToken, true);

        if (resume == null)
            throw new NotFoundException($"The resume with id {resumeId} was not found.");

        _accessControlService.EnsureApplicant(resume.UserId, _currentUser);

        await UploadResumeFileAsync(resume, uploadResumeFile.File, cancellationToken);
    }

    public async Task UploadResumeFileByUserIdAsync(
        Guid userId,
        UploadResumeFileRequestDto uploadResumeFile,
        CancellationToken cancellationToken = default)
    {
        if (uploadResumeFile?.File is null)
            throw new ValidationException("Resume file is required.");

        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(userId, cancellationToken);

        if (!isUserExist)
            throw new NotFoundException($"the user with id {userId} was not found");

        var resume = await _unitOfWork.ResumeRepository.GetResumeByUserIdAsync(userId, cancellationToken);

        if (resume == null)
            throw new NotFoundException($"the user with id {userId} dont have any resume.");

        _accessControlService.EnsureApplicant(resume.UserId, _currentUser);

        await UploadResumeFileAsync(resume, uploadResumeFile.File, cancellationToken);
    }

    #endregion

    #region Download Resume File Methods

    public async Task<AttachmentResponseDto> DownloadResumeFileByResumeIdAsync(
        Guid resumeId,
        CancellationToken cancellationToken)
    {
        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(resumeId, cancellationToken);

        if (resume == null)
            throw new NotFoundException($"The resume with id '{resumeId}' was not found.");

        if (resume.LastUploadedFileId == null)
            throw new NotFoundException($"The resume with id '{resume.Id}' does not have an attached file.");

        await EnsureUserCanAccessResumeAsync(resume.Id, resume.UserId, _currentUser, cancellationToken);

        return await _attachmentService.DownloadAsync(resume.LastUploadedFileId.Value);
    }

    public async Task<AttachmentResponseDto> DownloadResumeFileByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var resume = await _unitOfWork.ResumeRepository.GetResumeByUserIdAsync(userId, cancellationToken);

        if (resume == null)
            throw new NotFoundException($"The user with id '{userId}' does not have a resume.");

        if (resume.LastUploadedFileId == null)
            throw new NotFoundException($"The user with id '{userId}' does not have an attached file.");

        await EnsureUserCanAccessResumeAsync(resume.Id, resume.UserId, _currentUser, cancellationToken);

        return await _attachmentService.DownloadAsync(resume.LastUploadedFileId.Value, cancellationToken);
    }

    #endregion

    #region Private Methods

    private async Task EnsureUserCanAccessResumeAsync(Guid resumeId, Guid? targetUserId, ICurrentUser currentUser, CancellationToken cancellationToken)
    {
        // چک کردن اینکه اول ایای خودش داره درخواست میده یا نه
        var isSelfUser = targetUserId == currentUser.UserId;

        // ایا کارفرماس که داره درخواست میده 
        var isEmployer = currentUser.UserRoles.Contains(RoleConstants.EmployerRoleName);

        if (isSelfUser)
            return;

        if (isEmployer)
        {
            ///اینجا دارم چک میکنم که اگر کارفرما بود که داشت درخواست میداد 
            ///باید چک شه ایا این درخواستی که برای دیدن رزومه میده اصلا این رزومه برای کسی که درخواست رو برای اگهیش فرستاده یا نه 
            var hasJobApplication = await _unitOfWork.JobApplicationRepository
                                                        .CheckOwnerHasJobApplicationForResumeAsync(resumeId, currentUser.UserId, cancellationToken);
            ///حالا اگه برای اون درخواست بود ریترن میکنه
            if (hasJobApplication)
                return;
        }

        throw new ForbiddenException("You do not have sufficient access to view this resume.");
    }

    private async Task DeleteAttachmentAsync(Guid attachmentId, CancellationToken cancellationToken)
    {
        try
        {
            await _attachmentService.HardDeleteAttachmentAsync(attachmentId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete attachment {AttachmentId}", attachmentId);
        }
    }

    private async Task UploadResumeFileAsync(Resume resume, IFormFile file, CancellationToken cancellationToken)
    {
        //نگه داشتن ایدی قبلی فایل اپلود شده 
        var oldFileId = resume.LastUploadedFileId;
        Guid? newFileId = null;

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            newFileId = await _attachmentService.UploadAsync(file, AttachmentType.Document, cancellationToken);

            resume.UpdateFile(newFileId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            //اینجا برای این ترای کچ کذاشتم که اگه توی فلو اضافه کردن و اپدیت کردن فایل به رزومه به اکسپشن و مشکلی خورد....
            //و فایل جدیدی اپلود شده بود اما بدون اینکه به رزومه اختصاص داشته باشه اینو بیام حذف کنم 
            if (newFileId != null)
                await DeleteAttachmentAsync(newFileId.Value, cancellationToken);

            throw;
        }

        //حالا اگه فایل رزومه جدیدی سیو شد و اپدیت شد بیا اون فایل رزومه قدیمی رو حذف کن 
        if (oldFileId != null)
            await DeleteAttachmentAsync(oldFileId.Value, cancellationToken);
    }

    #endregion
}
