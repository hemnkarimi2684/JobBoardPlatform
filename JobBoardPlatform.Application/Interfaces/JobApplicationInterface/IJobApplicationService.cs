using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;

namespace JobBoardPlatform.Application.Interfaces.JobApplicationInterface;

public interface IJobApplicationService
{
    /// <summary>
    /// ثبت یک درخواست کار برای اگهی
    /// </summary>
    /// <param name="createCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateJobApplicationAsync(
        CreateJobApplicationRequestDto createCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت درخواست توسط شناسه اش
    /// </summary>
    /// <param name="jobApplicationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<JobApplicationDetailResponseDto> GetJobApplicationByIdAsync(
        Guid jobApplicationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت درخواست های یک اگهی 
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<JobApplicationDetailResponseDto>> GetAdvertisementJobApplicationsAsync(
        Guid advertisementId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// تغییر وضعیت درخواست
    /// </summary>
    /// <param name="jobApplicationId"></param>
    /// <param name="status"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateJobApplicationStatusAsync(
        Guid jobApplicationId,
        JobApplicationStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// مشاهده لیست تمام درخواست های خودم 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<JobApplicationDetailResponseDto>> GetJobApplicationsByUserIdAsync(
        Guid userId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// کنسل کردن درخواست کاری خود
    /// </summary>
    /// <param name="jobApplicationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CancelJobApplicationAsync(
        Guid jobApplicationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام وضعیت های درخوسات کاری در سیستم 
    /// </summary>
    /// <returns></returns>
    List<EnumResponseDto> GetJobApplicationStatuses();

    /// <summary>
    /// دریافت رزومه ای که با اون درخواست داده شده 
    /// </summary>
    /// <param name="jobApplicationId"></param>
    /// <param name="employerUserId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResumeDetailResponseDto> GetApplicantResumeByApplicationIdAsync(
    Guid jobApplicationId,
    Guid employerUserId,
    CancellationToken cancellationToken = default);
}
