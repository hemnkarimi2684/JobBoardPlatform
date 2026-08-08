using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;

namespace JobBoardPlatform.Mvc.Models.Advertisement;

public class AdvertisementEditViewModel : UpdateAdvertisementRequestDto
{
    public static AdvertisementEditViewModel FromResponseDto(AdvertisementDetailResponseDto source)
        => new()
        {
            Description = source.Description,
            MinimumAge = source.MinimumAge,
            MaximumAge = source.MaximumAge,
            MinimumSalary = source.MinimumSalary,
            MaximumSalary = source.MaximumSalary,
            ExperienceLevel = source.ExperienceLevel,
            CollaborationType = source.CollaborationType
        };
}
