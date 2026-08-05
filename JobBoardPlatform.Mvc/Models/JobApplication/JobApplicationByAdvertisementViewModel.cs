using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.JobApplication;

public class JobApplicationByAdvertisementViewModel : Pagination<JobApplicationDetailResponseDto>
{
    public Guid AdvertisementId { get; set; }

    public static JobApplicationByAdvertisementViewModel FromResponseDto(
        Pagination<JobApplicationDetailResponseDto> source,
        Guid advertisementId)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount,
            AdvertisementId = advertisementId
        };
}
