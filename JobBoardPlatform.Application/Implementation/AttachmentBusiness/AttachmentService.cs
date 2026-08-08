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

    public async Task<AttachmentResponseDto> DownloadAsync(
        Guid attachmentId,
        CancellationToken cancellationToken = default)
    {
        var attachment = await _unitOfWork.AttachmentRepository.GetAttachmentByIdAsync(a => new AttachmentResponseDto
        {
            AttachmentId = a.Id,
            FileName = a.FileName,
            ContentType = a.ContentType,
            Data = a.Data
        }, attachmentId, cancellationToken);

        if (attachment == null)
            throw new NotFoundException("attachment was not found");

        return attachment;
    }

    public async Task<bool> HardDeleteAttachmentAsync(
        Guid attachmentId,
        CancellationToken cancellationToken = default)
    {

        var result = await _unitOfWork.AttachmentRepository.HardDeleteAttachmentAsync(attachmentId, cancellationToken);

        if (!result)
            throw new NotFoundException("attachment was not found");

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<Guid> UploadAsync(
        IFormFile formFile,
        AttachmentType attachmentType,
        CancellationToken cancellationToken = default)
    {
        if (formFile == null)
            throw new ValidationException("file is required");

        using var stream = new MemoryStream();

        await formFile.CopyToAsync(stream);

        var attachment = new Attachment(formFile.FileName, attachmentType, formFile.ContentType, stream.ToArray(), _currentUser.UserId);

        await _unitOfWork.AttachmentRepository.AddAsync(attachment, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return attachment.Id;
    }
}
