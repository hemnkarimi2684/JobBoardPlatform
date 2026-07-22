using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.JobCategoryInterface;

public interface IJobCategoryService
{
    /// <summary>
    /// ساخت دسته بندی شغلی
    /// </summary>
    /// <param name="jobCategoryRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateJobCategoryAsync(
        CreateJobCategoryRequestDto jobCategoryRequestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام دسته بندی های شغلی 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<JobCategoryResponseDto>> GetAllJobCategoriesAsync(
        string text,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت دسته بندی شغلی توسط شناسه اش با شغل های در این دسته
    /// </summary>
    /// <param name="jobCategoryId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<JobCategoryDetailResponseDto> GetJobCategoryByIdAsync(
        Guid jobCategoryId,
        CancellationToken cancellationToken = default);
}
