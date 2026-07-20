using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;

public record UpdateAdvertisementRequestDto(
    [MinLength(100, ErrorMessage = "The description cannot be less than 100 characters.")]
    [MaxLength(2000, ErrorMessage = "The description cannot be more than 2000 characters.")]
    string? Description,

    [Range(18, 55, ErrorMessage = "Minimum age must be between 18 and 55.")]
    int? MinimumAge,

    [Range(18, 65, ErrorMessage = "Maximum age must be between 18 and 65.")]
    int? MaximumAge,

    [Range(0, double.MaxValue, ErrorMessage = "MinimumSalary must be greater than 0.")]
    decimal? MinimumSalary,

    [Range(0,  double.MaxValue, ErrorMessage = "MaximumSalary must be greater than 0.")]
    decimal? MaximumSalary,

    [Range(0, 50, ErrorMessage = "ExperienceLevel must be between 1 and 50.")]
    int? ExperienceLevel,

    [MaxLength(25, ErrorMessage = "CollaborationType cannot be more than 25 characters.")]
    string? CollaborationType
);
