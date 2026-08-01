using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using JobBoardPlatform.Core.Entities.UserEntity.ReadModels;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;

public class EmployerDetailResponseDto
{
    public Guid EmployerId { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public Guid CompanyId { get; init; }

    public string CompanyName { get; init; } = string.Empty;

    public DateTime EmployerCreatedAt { get; init; }

    public static List<EmployerDetailResponseDto> MapToResponseDto(IEnumerable<EmployerDetailReadModel> employerDetails)
    {
        return employerDetails.Select(ed => new EmployerDetailResponseDto
        {
            CompanyId = ed.CompanyId,
            Email = ed.Email,
            EmployerId = ed.EmployerId,
            PhoneNumber = ed.PhoneNumber,
            CompanyName = ed.CompanyName,
            EmployerCreatedAt = ed.EmployerCreatedAt
        }).ToList();
    }
}
