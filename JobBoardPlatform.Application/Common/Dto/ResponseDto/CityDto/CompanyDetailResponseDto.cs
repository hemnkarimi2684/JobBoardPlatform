namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;

public record CompanyDetailResponseDto(
    string CityName,
    string CompanyName,
    DateTime YearOfEstablishment,
    string Industry,
    string AboutUs
);

