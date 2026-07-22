using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;

public class AdvertisementSearchRequestDto
{
    [StringLength(100, ErrorMessage = "Search term cannot be longer than 100 characters.")]
    public string? SearchTerm { get; set; }
}
