using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Implementation;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.EducationDetailDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.EducationDetailInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.EducationDetailBusiness;

public class EducationDetailService : IEducationDetailService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    public EducationDetailService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
    }

    #region Create Methods

    public async Task<bool> CreateEducationDetailAsync(
        CreateEducationDetailRequestDto createCommand,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureApplicantOrAdmin(createCommand.UserId, _currentUser);

        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(createCommand.UserId, cancellationToken);

        if (!isUserExist)
            throw new NotFoundException($"user with id {createCommand.UserId} was not found");

        var educationDetail = new EducationDetail(
                                           createCommand.CertificateDegree,
                                           createCommand.Major,
                                           createCommand.University,
                                           createCommand.StartDate,
                                           createCommand.CompletionDate,
                                           createCommand.Percentage,
                                           createCommand.IsCurrentlyStudying,
                                           createCommand.UserId,
                                           _currentUser.UserId
                                           );

        await _unitOfWork.EducationDetailRepository.AddAsync(educationDetail, cancellationToken);

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Get Methods 

    public async Task<Pagination<EducationHistoryResponseDto>> GetUserEducationDetailsAsync(
        Guid userId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        var (userEducationDetails, totalDataCount) = await _unitOfWork.EducationDetailRepository
                                                          .GetUserEducationDetailsAsync(ed =>
                                                          new EducationHistoryResponseDto
                                                          {
                                                              EducationDetailId = ed.Id,
                                                              UserId = ed.UserId,
                                                              CertificateDegreeName = ed.CertificateDegreeName,
                                                              Major = ed.Major,
                                                              University = ed.University,
                                                              StartDate = ed.StartDate,
                                                              CompletionDate = ed.CompletionDate,
                                                              Percentage = ed.Percentage,
                                                              IsCurrentlyStudying = ed.IsCurrentlyStudying
                                                          },
                                                          userId,
                                                          cancellationToken,
                                                          pagingCommand.PageNumber,
                                                          pagingCommand.PageSize);

        return Pagination<EducationHistoryResponseDto>
                    .GetPagination(userEducationDetails,
                                   pagingCommand.PageNumber,
                                   pagingCommand.PageSize,
                                   totalDataCount);
    }

    public async Task<EducationHistoryResponseDto> GetEducationDetailByIdAsync(
        Guid educationDetailId,
        CancellationToken cancellationToken = default)
    {
        var educationDetail = await _unitOfWork.EducationDetailRepository.GetByIdAsync(educationDetailId, cancellationToken);

        if (educationDetail == null)
            throw new NotFoundException($"The education detail with id {educationDetailId} was not found.");

        _accessControlService.EnsureApplicantOrAdmin(educationDetail.UserId, _currentUser);

        return new EducationHistoryResponseDto
        {
            EducationDetailId = educationDetail.Id,
            UserId = educationDetail.UserId,
            CertificateDegreeName = educationDetail.CertificateDegreeName,
            Major = educationDetail.Major,
            University = educationDetail.University,
            StartDate = educationDetail.StartDate,
            CompletionDate = educationDetail.CompletionDate,
            Percentage = educationDetail.Percentage,
            IsCurrentlyStudying = educationDetail.IsCurrentlyStudying
        };
    }

    #endregion

    #region Update Methods

    public async Task<bool> UpdateEducationDetailAsync(
        Guid educationDetailId,
        UpdateEducationDetailRequestDto updateCommand,
        CancellationToken cancellationToken = default)
    {
        var userId = await _unitOfWork.EducationDetailRepository.GetEducationDetailUserIdAsync(educationDetailId, cancellationToken);

        if (userId == null)
            throw new NotFoundException($"The education detail with id {educationDetailId} was not found.");

        _accessControlService.EnsureApplicantOrAdmin(userId.Value, _currentUser);

        var result = await _unitOfWork.EducationDetailRepository.UpdateEducationDetailAsync(
                                                                                            educationDetailId,
                                                                                            cancellationToken,
                                                                                            MapToUpdateEducationDetail(updateCommand));

        if (!result)
            throw new NotFoundException($"the educationDetail with id {educationDetailId} was not found");

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Private Methods

    private UpdateEducationDetail MapToUpdateEducationDetail(UpdateEducationDetailRequestDto updateCommand)
    {
        return new UpdateEducationDetail
        {
            CertificateDegreeName = updateCommand.CertificateDegree,
            Major = updateCommand.Major,
            University = updateCommand.University,
            StartDate = updateCommand.StartDate,
            CompletionDate = updateCommand.CompletionDate,
            Percentage = updateCommand.Percentage < 1 ? null : updateCommand.Percentage,
            IsCurrentlyStudying = updateCommand.IsCurrentlyStudying,
            ModifiedById = _currentUser.UserId
        };
    }

    #endregion
}
