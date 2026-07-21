using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;

public class UpdateProfileRequestDto
{
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters.")]
    public string? FirstName { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters.")]
    public string? LastName { get; set; }

    [StringLength(1000, ErrorMessage = "Bio must not exceed 1000 characters.")]
    public string? Bio { get; set; }

    [StringLength(300, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 300 characters.")]
    public string? Address { get; set; }

    public DateTime? BirthDate { get; set; }

    public Guid? CityId { get; set; }

    public Gender? Gender { get; set; }
}

