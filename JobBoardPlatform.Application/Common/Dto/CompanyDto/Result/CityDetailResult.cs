namespace JobBoardPlatform.Application.Common.Dto.CompanyDto.Result;

public record CityDetailResult(
    string CityName,
    int CityCode,
    string ProvinceName,
    int ProvinceCode);

