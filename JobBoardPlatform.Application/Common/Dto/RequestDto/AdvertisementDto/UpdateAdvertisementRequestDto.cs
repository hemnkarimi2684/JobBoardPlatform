using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;

public class UpdateAdvertisementRequestDto
{
    [MinLength(100, ErrorMessage = "Description cannot be less than 100 characters.")]
    [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    public string? Description { get; set; }

    [Range(18, 55, ErrorMessage = "Minimum age must be between 18 and 55.")]
    public int? MinimumAge { get; set; }

    [Range(18, 65, ErrorMessage = "Maximum age must be between 18 and 65.")]
    public int? MaximumAge { get; set; }

    [Range(typeof(decimal), "0", "79228162514264337593543950335",
        ErrorMessage = "Minimum salary must be greater than or equal to 0.")]
    public decimal? MinimumSalary { get; set; }

    [Range(typeof(decimal), "0", "79228162514264337593543950335",
        ErrorMessage = "Maximum salary must be greater than or equal to 0.")]
    public decimal? MaximumSalary { get; set; }

    public int? ExperienceLevel { get; set; }

    public CollaborationType? CollaborationType { get; set; }
}