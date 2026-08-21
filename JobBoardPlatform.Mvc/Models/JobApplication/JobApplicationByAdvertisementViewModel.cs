using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.JobApplication;

public class JobApplicationByAdvertisementViewModel : Pagination<JobApplicationDetailResponseDto>
{
    public Guid AdvertisementId { get; set; }

    public AdvertisementStatus AdvertisementStatus { get; set; }

    public static JobApplicationByAdvertisementViewModel FromResponseDto(
        Pagination<JobApplicationDetailResponseDto> source,
        Guid advertisementId,
        AdvertisementStatus advertisementStatus)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount,
            AdvertisementId = advertisementId,
            AdvertisementStatus = advertisementStatus
        };
}
