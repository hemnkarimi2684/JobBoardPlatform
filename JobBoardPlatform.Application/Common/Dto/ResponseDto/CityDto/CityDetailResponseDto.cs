namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;

public record CityDetailResponseDto(
    Guid CityId,
    string CityName,
    int CityCode,
    string ProvinceName,
    Guid ProvinceId,
    int ProvinceCode);

