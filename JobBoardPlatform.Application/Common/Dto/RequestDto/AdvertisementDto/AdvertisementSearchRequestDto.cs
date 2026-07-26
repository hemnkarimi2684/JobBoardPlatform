using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;

public class AdvertisementSearchRequestDto
{
    [MaxLength(100, ErrorMessage = "search term cannot be longer than 100 characters.")]
    public string? SearchTerm { get; set; }
}
