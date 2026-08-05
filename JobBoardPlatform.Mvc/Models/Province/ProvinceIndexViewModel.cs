using JobBoardPlatform.Application.Common.Dto.ResponseDto.ProvinceDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Province;

public class ProvinceIndexViewModel : Pagination<ProvinceResponseDto>
{
    public static ProvinceIndexViewModel FromResponseDto(Pagination<ProvinceResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
