using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.ProvinceDto;

public class CreateProvinceRequestDto
{
    [Required(ErrorMessage = "Prvince name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Province name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Province code is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Province code must be a positive number.")]
    public int Code { get; set; }
}
