using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class CitiesViewModel
{
    public List<CityDetailResponseDto> Cities { get; set; } = new();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalPageCount { get; set; }

    public static CitiesViewModel FromResponseDto(
        Pagination<CityDetailResponseDto> response)
    {
        return new CitiesViewModel
        {
            Cities = response.Data,
            PageNumber = response.PageNumber,
            PageSize = response.PageSize,
            TotalPageCount = response.TotalPageCount
        };
    }
}
