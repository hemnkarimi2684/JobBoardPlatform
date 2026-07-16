using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AttachmentInterface;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;


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
            FileName = a.FileName,
            ContentType = a.ContentType,
            Data = a.Data
        }, attachmentId);

        if (attachment == null)
            throw new NotFoundException($"the attachment with id {attachmentId} was not found");

        return attachment;
    }

    public async Task<Guid> UploadAsync(IFormFile formFile)
    {
        if (formFile == null)
            throw new ValidationException("file is required");

        using var stream = new MemoryStream();

        await formFile.CopyToAsync(stream);

        var attachment = new Attachment(formFile.FileName, formFile.ContentType, stream.ToArray());

        await _unitOfWork.AttachmentRepository.AddAsync(attachment);

        return attachment.Id;
    }
}
