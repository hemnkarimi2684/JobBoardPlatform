using JobBoardPlatform.Application.Dto.CompanyDto.Command;

namespace JobBoardPlatform.Application.Interfaces.CompanyInterface;

public interface ICompanyService
{
    Task<bool> CreateCompanyAsync(CreateCompanyCommand createCompanyCommand);
}
