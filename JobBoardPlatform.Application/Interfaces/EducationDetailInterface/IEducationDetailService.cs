using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.EducationDetailDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.EducationDetailInterface;

public interface IEducationDetailService
{
    /// <summary>
    /// دریافت مدرک های تحصیلی کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Pagination<UserEducationDetailResponseDto>> GetUserEducationDetailsAsync(Guid userId, PagingRequestDto pagingCommand);

    /// <summary>
    /// ثبت مدرک تحصیلی 
    /// </summary>
    /// <param name="createCommand"></param>
    /// <returns></returns>
    Task<bool> CreateEducationDetailAsync(CreateEducationDetailRequestDto createCommand);

    /// <summary>
    /// اپدیت اطلاعات مدرک تحصیلی ثبت شده 
    /// </summary>
    /// <param name="educationDetailId"></param>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateEducationDetailAsync(Guid educationDetailId, UpdateEducationDetailRequestDto updateCommand);
}
