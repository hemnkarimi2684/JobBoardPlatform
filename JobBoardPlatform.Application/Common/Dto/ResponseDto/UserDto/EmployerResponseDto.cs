using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;

public class EmployerResponseDto
{
    public Guid EmployerId { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public Guid CompanyId { get; init; }
}
