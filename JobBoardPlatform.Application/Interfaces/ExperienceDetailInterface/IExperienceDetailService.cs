using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ExperienceDetailDto;
using JobBoardPlatform.Core.Entities.Common.Dto;
using Microsoft.AspNetCore.Routing;

namespace JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;

public interface IExperienceDetailService
{
    /// <summary>
    /// دریافت سابقه کار های یک کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<UserExperienceDetailResponseDto>> GetUserExperienceDetailsAsync(
        Guid userId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ثبت یک تجربه کاری
    /// </summary>
    /// <param name="createCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CreateExperienceDetailAsync(
        CreateExperienceDetailRequestDto createCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اپدیت یک تجربه کاری
    /// </summary>
    /// <param name="experienceDetailId"></param>
    /// <param name="updateCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UpdateExperienceDetailAsync(
        Guid experienceDetailId,
        UpdateExperienceDetailRequestDto updateCommand,
        CancellationToken cancellationToken = default);
}
