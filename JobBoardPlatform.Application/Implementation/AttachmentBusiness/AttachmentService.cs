using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AttachmentInterface;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Data;
using Microsoft.AspNetCore.Http;


namespace JobBoardPlatform.Application.Implementation.AttachmentBusiness;

public class AttachmentService : IAttachmentService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    public AttachmentService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<AttachmentResponseDto> DownloadAsync(Guid attachmentId)
    {
        var attachment = await _unitOfWork.AttachmentRepository.GetAttachmentByIdAsync(a => new AttachmentResponseDto
        {
            AttachmentId = a.Id,
            FileName = a.FileName,
            ContentType = a.ContentType,
            Data = a.Data
        }, attachmentId);

        if (attachment == null)
            throw new NotFoundException($"the attachment with id {attachmentId} was not found");

        return attachment;
    }

    public async Task<bool> HardDeleteAttachmentAsync(Guid attachmentId)
    {

        var result = await _unitOfWork.AttachmentRepository.HardDeleteAttachmentAsync(attachmentId);

        if (!result)
            throw new NotFoundException($"the attachment with id {attachmentId} was not found");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<Guid> UploadAsync(IFormFile formFile, AttachmentType attachmentType)
    {
        if (formFile == null)
            throw new ValidationException("file is required");

        using var stream = new MemoryStream();

        await formFile.CopyToAsync(stream);

        var attachment = new Attachment(formFile.FileName, attachmentType, formFile.ContentType, stream.ToArray(), _currentUser.UserId);

        await _unitOfWork.AttachmentRepository.AddAsync(attachment);

        await _unitOfWork.SaveChangesAsync();

        return attachment.Id;
    }
}
