using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.JobInterface;

public interface IJobService
{
    /// <summary>
    /// ساخت شغل
    /// </summary>
    /// <param name="jobRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateJobAsync(
        CreateJobRequestDto jobRequestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام شغل ها
    /// </summary>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<JobResponseDto>> GetAllJobsAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت اگهی های شغل
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<JobAdvertisementListItemResponseDto>> GetJobAdvertisementsAsync(
        Guid jobId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);
}
