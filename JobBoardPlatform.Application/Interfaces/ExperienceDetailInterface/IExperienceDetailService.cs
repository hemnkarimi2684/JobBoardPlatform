using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ExperienceDetailDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;

public interface IExperienceDetailService
{
    /// <summary>
    /// دریافت سابقه کار های یک کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Pagination<UserExperienceDetailResponseDto>> GetUserExperienceDetailsAsync(Guid userId, PagingRequestDto pagingCommand);

    /// <summary>
    /// ثبت یک تجربه کاری 
    /// </summary>
    /// <param name="createCommand"></param>
    /// <returns></returns>
    Task<bool> CreateExperienceDetailAsync(CreateExperienceDetailRequestDto createCommand);

    /// <summary>
    /// اپدیت یک تجربه کاری 
    /// </summary>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateExperienceDetailAsync(Guid experienceDetailId, UpdateExperienceDetailRequestDto updateCommand);
}
