using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;

namespace JobBoardPlatform.Application.Interfaces.JobApplicationInterface;

public interface IJobApplicationService
{
    /// <summary>
    /// ثبت یک درخواست کار برای اگهی
    /// </summary>
    /// <param name="createCommand"></param>
    /// <returns></returns>
    Task<bool> CreateJobApplicationAsync(CreateJobApplicationRequestDto createCommand);

    /// <summary>
    /// دریافت درخواست توسط شناسه اش 
    /// </summary>
    /// <param name="jobApplicationId"></param>
    /// <returns></returns>
    Task<JobApplicationInfoResponseDto> GetJobApplicationByIdAsync(Guid jobApplicationId);

    /// <summary>
    /// دریافت درخواست های یک اگهی 
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<Pagination<JobApplicationInfoResponseDto>> GetAdvertisementJobApplicationsAsync(Guid advertisementId, PagingRequestDto pagingCommand);

    /// <summary>
    /// تغییر وضعیت درخواست
    /// </summary>
    /// <param name="jobApplicationId"></param>
    /// <param name="statusName"></param>
    /// <returns></returns>
    Task<bool> UpdateJobApplicationStatusAsync(Guid jobApplicationId, JobApplicationStatus status);
}
