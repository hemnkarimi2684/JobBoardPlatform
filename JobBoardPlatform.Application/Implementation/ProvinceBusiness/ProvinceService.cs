using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ProvinceDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ProvinceDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.ProvinceInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.ProvinceBusiness;

public class ProvinceService : IProvinceService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    public ProvinceService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
    }

    #region Create Methods 

    public async Task CreateProvinceAsync(
        CreateProvinceRequestDto provinceRequestDto,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var isDuplicateNameOrCode = await _unitOfWork.ProvinceRepository
                                                    .IsDuplicateNameOrCodeAsync(provinceRequestDto.Name, provinceRequestDto.Code, cancellationToken);

        if (isDuplicateNameOrCode)
            throw new ConflictException("A province with the same name or code already exists.");

        var province = new Province(provinceRequestDto.Name, provinceRequestDto.Code, _currentUser.UserId);

        await _unitOfWork.ProvinceRepository.AddAsync(province, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Get Methods 

    public async Task<Pagination<ProvinceResponseDto>> GetAllProvincesAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var (result, totalDataCount) = await _unitOfWork.ProvinceRepository.GetAllProvincesAsync(p => new ProvinceResponseDto
        {
            ProvinceId = p.Id,
            Code = p.ProvinceCode,
            Name = p.Name
        },
        textRequestDto.Text,
        cancellationToken,
        pagingCommand.PageNumber,
        pagingCommand.PageSize);

        return Pagination<ProvinceResponseDto>.GetPagination(result, pagingCommand.PageNumber, pagingCommand.PageSize, totalDataCount);
    }

    #endregion
}
