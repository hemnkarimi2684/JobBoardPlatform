using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;

namespace JobBoardPlatform.Mvc.Models.Company;

public class CompanyMyCompanyViewModel : EmployerWithCompanyResponseDto
{
    public static CompanyMyCompanyViewModel FromResponseDto(EmployerWithCompanyResponseDto source)
        => new()
        {
            CompanyId = source.CompanyId,
            Name = source.Name,
            UserId = source.UserId,
            PhoneNumber = source.PhoneNumber,
            Email = source.Email,
            YearOfEstablishment = source.YearOfEstablishment,
            JobCategoryName = source.JobCategoryName,
            JobCategoryId = source.JobCategoryId,
            AboutUs = source.AboutUs,
            WebSiteAddress = source.WebSiteAddress,
            OwnershipType = source.OwnershipType,
            CompanySize = source.CompanySize,
            ActivityType = source.ActivityType,
            CompanyImageFileId = source.CompanyImageFileId,
            Cities = source.Cities
        };
}
