using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Advertisement;

public class AdvertisementMyAdsViewModel : Pagination<AdvertisementDetailResponseDto>
{
    public static AdvertisementMyAdsViewModel FromResponseDto(Pagination<AdvertisementDetailResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
