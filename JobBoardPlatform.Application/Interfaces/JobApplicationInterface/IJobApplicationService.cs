using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Dto.JobApplicationDto.Command;
using JobBoardPlatform.Application.Common.Dto.JobApplicationDto.Result;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.JobApplicationInterface;

public interface IJobApplicationService
{
    /// <summary>
    /// ثبت یک درخواست کار برای اگهی
    /// </summary>
    /// <param name="createCommand"></param>
    /// <returns></returns>
    Task<bool> CreateJobApplicationAsync(CreateJobApplicationCommand createCommand);

    /// <summary>
    /// دریافت درخواست توسط شناسه اش 
    /// </summary>
    /// <param name="jobApplicationId"></param>
    /// <returns></returns>
    Task<JobApplicationInfoResult> GetJobApplicationByIdAsync(Guid jobApplicationId);

    /// <summary>
    /// دریافت درخواست های یک اگهی 
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<Pagination<JobApplicationInfoResult>> GetAdvertisementJobApplicationsAsync(Guid advertisementId, PagingCommand pagingCommand);

    /// <summary>
    /// تغییر وضعیت درخواست
    /// </summary>
    /// <param name="jobApplicationId"></param>
    /// <param name="statusName"></param>
    /// <returns></returns>
    Task<bool> UpdateJobApplicationStatusAsync(Guid jobApplicationId, string statusName);
}
