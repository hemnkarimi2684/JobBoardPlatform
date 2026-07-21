namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;

public class CityDetailResponseDto
{
    public Guid CityId { get; init; }

    public string CityName { get; init; } = string.Empty;

    public int CityCode { get; init; }

    public string ProvinceName { get; init; } = string.Empty;

    public Guid ProvinceId { get; init; }

    public int ProvinceCode { get; init; }
}

