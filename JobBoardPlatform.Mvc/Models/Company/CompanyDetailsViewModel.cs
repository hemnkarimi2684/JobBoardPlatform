using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

namespace JobBoardPlatform.Mvc.Models.Company;

public class CompanyDetailsViewModel : CompanyDetailResponseDto
{
    public static CompanyDetailsViewModel FromResponseDto(CompanyDetailResponseDto source)
        => new()
        {
            Id = source.Id,
            Name = source.Name,
            UserId = source.UserId,
            YearOfEstablishment = source.YearOfEstablishment,
            JobCategoryId = source.JobCategoryId,
            JobCategoryName = source.JobCategoryName,
            AboutUs = source.AboutUs,
            WebSiteAddress = source.WebSiteAddress,
            OwnershipType = source.OwnershipType,
            CompanySize = source.CompanySize,
            ActivityType = source.ActivityType,
            CompanyImageFileId = source.CompanyImageFileId,
            Cities = source.Cities
        };
}
