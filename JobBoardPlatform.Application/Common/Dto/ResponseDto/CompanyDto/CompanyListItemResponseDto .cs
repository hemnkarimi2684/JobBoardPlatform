namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

public class CompanyListItemResponseDto
{
    public Guid CompanyId { get; init; }

    public Guid OwnedByUserId { get; init; }

    public string CityName { get; init; } = string.Empty;

    public Guid CityId { get; init; }

    public string CompanyName { get; init; } = string.Empty;

    public DateTime YearOfEstablishment { get; init; }

    public string Industry { get; init; } = string.Empty;

    public string AboutUs { get; init; } = string.Empty;

    public Guid? CompanyImageFileId { get; init; }
}
