using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;

namespace JobBoardPlatform.Mvc.Models.Advertisement;

public class AdvertisementDetailsViewModel : AdvertisementDetailResponseDto
{
    public static AdvertisementDetailsViewModel FromResponseDto(AdvertisementDetailResponseDto source)
        => new()
        {
            AdvertisementId = source.AdvertisementId,
            JobId = source.JobId,
            CityId = source.CityId,
            CompanyId = source.CompanyId,
            Description = source.Description,
            MinimumAge = source.MinimumAge,
            MaximumAge = source.MaximumAge,
            MinimumSalary = source.MinimumSalary,
            MaximumSalary = source.MaximumSalary,
            ExperienceLevel = source.ExperienceLevel,
            CreatedAt = source.CreatedAt,
            CollaborationType = source.CollaborationType,
            CityName = source.CityName,
            CompanyName = source.CompanyName,
            JobName = source.JobName,
            AboutCompany = source.AboutCompany,
            CompanyJobCategoryId = source.CompanyJobCategoryId,
            CompanyJobCategoryName = source.CompanyJobCategoryName,
            IsFeatured = source.IsFeatured,
            FeaturedUntil = source.FeaturedUntil,
            IsActive = source.IsActive,
            IsOwner = source.IsOwner,
            SkillNames = source.SkillNames,
            CompanyImageFileId = source.CompanyImageFileId
        };
}
