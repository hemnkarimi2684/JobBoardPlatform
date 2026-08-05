using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Advertisement;

public class AdvertisementFilterViewModel : Pagination<AdvertisementDetailResponseDto>
{
    public static AdvertisementFilterViewModel FromResponseDto(Pagination<AdvertisementDetailResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
