using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.CityDto;

public class CreateCityRequestDto
{
    [Required(ErrorMessage = "City name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "City name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "City code is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "City code must be a positive number.")]
    public int Code { get; set; }

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid ProvinceId { get; set; }
}
