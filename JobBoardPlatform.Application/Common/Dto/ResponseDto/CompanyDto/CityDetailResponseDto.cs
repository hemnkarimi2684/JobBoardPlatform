namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

public record CityDetailResponseDto(
    string CityName,
    int CityCode,
    string ProvinceName,
    int ProvinceCode);

