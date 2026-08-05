using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

namespace JobBoardPlatform.Mvc.Models.Company;

public class CompanyEditViewModel : UpdateCompanyInfoRequestDto
{
    public static CompanyEditViewModel FromResponseDto(CompanyDetailResponseDto source)
        => new()
        {
            Name = source.Name,
            YearOfEstablishment = source.YearOfEstablishment,
            AboutUs = source.AboutUs,
            WebSiteAddress = source.WebSiteAddress,
            OwnershipType = source.OwnershipType,
            CompanySize = source.CompanySize,
            JobCategoryId = source.JobCategoryId,
            ActivityType = source.ActivityType
        };
}
