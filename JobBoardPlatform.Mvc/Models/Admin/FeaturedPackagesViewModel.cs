using JobBoardPlatform.Application.Common.Dto.ResponseDto.FeaturedPackageDto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class FeaturedPackagesViewModel
{
    public List<FeaturedPackageResponseDto> Packages { get; set; } = new();

    public static FeaturedPackagesViewModel FromResponseDto(List<FeaturedPackageResponseDto> source)
        => new()
        {
            Packages = source
        };
}
