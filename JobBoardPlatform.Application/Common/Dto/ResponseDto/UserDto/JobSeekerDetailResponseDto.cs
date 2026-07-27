using JobBoardPlatform.Core.Entities.UserEntity.ReadModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;

public class JobSeekerDetailResponseDto
{
    public Guid Id { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public DateTime CreatedAt { get; init; }

    public static List<JobSeekerDetailResponseDto> MapToResponseDto(IEnumerable<JobSeekerDetailReadModel> jobSeekerDetails)
    {
        return jobSeekerDetails.Select(jd => new JobSeekerDetailResponseDto
        {
            IsActive = jd.IsActive,
            Email = jd.Email,
            Id = jd.Id,
            PhoneNumber = jd.PhoneNumber,
            CreatedAt = jd.CreatedAt,
        }).ToList();
    }
}
