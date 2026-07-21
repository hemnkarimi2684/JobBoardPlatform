using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;

public class AdvertisementSearchRequestDto
{
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Job title must be between 2 and 120 characters.")]
    public string? JobTitle { get; set; }

    public CollaborationType? CollaborationType { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "City name must be between 2 and 100 characters.")]
    public string? CityName { get; set; }
}
