using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.City;

public class CityDetailsViewModel : Pagination<CompanyListItemResponseDto>
{
    public static CityDetailsViewModel FromResponseDto(Pagination<CompanyListItemResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
