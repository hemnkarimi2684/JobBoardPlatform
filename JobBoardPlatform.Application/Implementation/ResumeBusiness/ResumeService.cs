using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AttachmentInterface;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using Microsoft.Extensions.Logging;

namespace JobBoardPlatform.Application.Implementation.ResumeBusiness;


public class ResumeService : IResumeService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAttachmentService _attachmentService;

    private readonly ILogger<ResumeService> _logger;

    public ResumeService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAttachmentService attachmentService, ILogger<ResumeService> logger)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _attachmentService = attachmentService;
        _logger = logger;
    }

    public async Task<bool> CreateResumeAsync(CreateResumeRequestDto resumeCommand)
    {
        var isExistUser = await _unitOfWork.UserRepository.IsUserExistAsync(resumeCommand.UserId);

        if (!isExistUser)
            throw new NotFoundException($"the user with id {resumeCommand.UserId} was not found");

        CheckSelfOrAdminPermission(resumeCommand.UserId, _currentUser);

        var isDuplicateResumeFortUser = await _unitOfWork.ResumeRepository.IsDuplicateResumeForUserAsync(resumeCommand.UserId);

        if (isDuplicateResumeFortUser)
            throw new ConflictException($"the user with id {resumeCommand.UserId} already has resume");

        var hasEducationDetail = await _unitOfWork.EducationDetailRepository.UserHasEducationDetailAsync(resumeCommand.UserId);

        if (!hasEducationDetail)
            throw new ValidationException("the user must have education detail for register resume");

        var resume = new Resume(resumeCommand.Title, resumeCommand.UserId, null, _currentUser.UserId);

        await _unitOfWork.ResumeRepository.AddAsync(resume);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<ResumeDetailResponseDto> GetResumeByUserIdAsync(Guid userId)
    {
        CheckSelfOrAdminPermission(userId, _currentUser);

        var result = await _unitOfWork.ResumeRepository.GetResumeByUserIdAsync(r => new ResumeDetailResponseDto
                                                                              (
                                                                                 r.Title,
                                                                                 r.UserId
                                                                              ), userId);

        if (result == null)
            throw new NotFoundException($"the resume with id {userId} was not found");

        return result;
    }

    public async Task UploadResumeFileAsync(Guid resumeId, UploadResumeFileRequestDto uploadResumeFile)
    {
        if (uploadResumeFile?.File is null)
            throw new ValidationException("Image file is required.");

        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(resumeId);

        if (resume == null)
            throw new NotFoundException($"The resume with id {resumeId} was not found.");

        CheckSelfOrAdminPermission(resume.UserId, _currentUser);

        //نگه داشتن ایدی قبلی فایل اپلود شده 
        var oldFileId = resume.LastUploadedFileId;
        Guid? newFileId = null;

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            newFileId = await _attachmentService.UploadAsync(uploadResumeFile.File, AttachmentType.Document);

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

    public async Task<AttachmentResponseDto> DownloadResumeFileAsync(Guid resumeId)
    {
        var resumeFileId = await _unitOfWork.ResumeRepository.GetResumeFileIdAsync(resumeId);

        if (resumeFileId == null)
            throw new NotFoundException($"The resume with id '{resumeId}' does not have an attached file.");

        return await _attachmentService.DownloadAsync(resumeFileId.Value);
    }


    #region Private Methods

    private void CheckSelfOrAdminPermission(Guid? targetUserId, ICurrentUser currentUser)
    {
        var isSelfUser = targetUserId == currentUser.UserId;

        var isAdmin = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        //اینجا چک میشه که کاربر فقط بتونه خودش اطلاعات مدرک تحصیلیش رو اپدیت کنه نه کس دیگه ای به جز ادمین                                                               
        if (!isAdmin && !isSelfUser)
            throw new ForbiddenException("You do not have sufficient access to manage this resume.");
    }

    private void CheckAdminPermission(ICurrentUser currentUser)
    {
        var isAdminOrEmployer = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        if (!isAdminOrEmployer)
            throw new ForbiddenException("You do not have sufficient access to manage a resume.");
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

    #endregion
}
