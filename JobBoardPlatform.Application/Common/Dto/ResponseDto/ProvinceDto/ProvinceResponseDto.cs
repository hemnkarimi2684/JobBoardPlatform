namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.ProvinceDto;

public class ProvinceResponseDto
{
    public Guid ProvinceId { get; init; }

    public string Name { get; init; } = default!;

    public int Code { get; init; }
}
