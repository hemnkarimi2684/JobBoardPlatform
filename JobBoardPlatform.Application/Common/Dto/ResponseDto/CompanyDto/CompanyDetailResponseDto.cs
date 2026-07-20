namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

public record CompanyDetailResponseDto(
    Guid CompanyId,
    Guid OwnedByUserId,
    string CityName,
    string CompanyName,
    DateTime YearOfEstablishment,
    string Industry,
    string AboutUs,
    Guid? CompanyImageFileId
);

