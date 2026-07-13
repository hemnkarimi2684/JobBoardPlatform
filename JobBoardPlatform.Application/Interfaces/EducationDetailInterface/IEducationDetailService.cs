using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Dto.EducationDetailDto.Command;
using JobBoardPlatform.Application.Common.Dto.EducationDetailDto.Result;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.EducationDetailInterface;

public interface IEducationDetailService
{
    /// <summary>
    /// دریافت مدرک های تحصیلی کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Pagination<UserEducationDetailResult>> GetUserEducationDetailsAsync(Guid userId, PagingCommand pagingCommand);

    /// <summary>
    /// ثبت مدرک تحصیلی 
    /// </summary>
    /// <param name="createCommand"></param>
    /// <returns></returns>
    Task<bool> CreateEducationDetailAsync(CreateEducationDetailCommand createCommand);

    /// <summary>
    /// اپدیت اطلاعات مدرک تحصیلی ثبت شده 
    /// </summary>
    /// <param name="educationDetailId"></param>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateEducationDetailAsync(Guid educationDetailId, UpdateEducationDetailCommand updateCommand);
}
