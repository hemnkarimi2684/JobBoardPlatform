using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Dto.ExperienceDetailDto.Command;
using JobBoardPlatform.Application.Common.Dto.ExperienceDetailDto.Result;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;

public interface IExperienceDetailService
{
    /// <summary>
    /// دریافت سابقه کار های یک کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Pagination<UserExperienceDetailResult>> GetUserExperienceDetailsAsync(Guid userId, PagingCommand pagingCommand);

    /// <summary>
    /// ثبت یک تجربه کاری 
    /// </summary>
    /// <param name="createCommand"></param>
    /// <returns></returns>
    Task<bool> CreateExperienceDetailAsync(CreateExperienceDetailCommand createCommand);

    /// <summary>
    /// اپدیت یک تجربه کاری 
    /// </summary>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateExperienceDetailAsync(Guid experienceDetailId, UpdateExperienceDetailCommand updateCommand);
}
