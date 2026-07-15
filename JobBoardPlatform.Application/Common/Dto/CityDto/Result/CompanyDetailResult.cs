namespace JobBoardPlatform.Application.Common.Dto.CityDto.Result;

public record CompanyDetailResult(
    string CityName,
    string CompanyName,
    DateTime YearOfEstablishment,
    string Industry,
    string AboutUs
);

