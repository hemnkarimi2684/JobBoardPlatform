using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;

public class CreateProfileRequestDto
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters.")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Bio must not exceed 1000 characters.")]
    public string Bio { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(300, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 300 characters.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid UserId { get; set; }

    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid CityId { get; set; }

    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }
}
