using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class CitiesViewModel
{
    public List<CityDetailResponseDto> Cities { get; set; } = new();

    public static CitiesViewModel FromResponseDto(List<CityDetailResponseDto> cities)
        => new() { Cities = cities };
}
