using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Dto.CompanyDto.Command;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using JobBoardPlatform.Core.Entities.StatusEntity.Entity;
using System.Data;

namespace JobBoardPlatform.Application.Implementation.CompanyBusiness;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CreateCompanyAsync(CreateCompanyCommand createCommand)
    {
        var doesCityExist = await _unitOfWork.CityRepository.IsCityExistAsync(createCommand.CityId);

        if (!doesCityExist)
            throw new NotFoundException($"City with id {createCommand.CityId} was not found.");

        var companyExistsByName = await _unitOfWork.CompanyRepository.IsCompanyExistByNameAsync(createCommand.Name);

        if (companyExistsByName)
            throw new ConflictException($"the company with this name {createCommand.Name} already exist");

        var companyExistsForOwner = await _unitOfWork.CompanyRepository.IsCompanyExistForOwnerId(createCommand.OwnedByUserId);

        if (companyExistsForOwner)
            throw new ConflictException($"this owner already has company");

        var parsedEnums = ParseEnums(createCommand.OwnershipType, createCommand.CompanySize);

        var company = new Company(
            createCommand.Name, createCommand.YearOfEstablishment, createCommand.Industry,
            createCommand.AboutUs, createCommand.WebSiteAddress, parsedEnums.Item1,
            createCommand.OwnedByUserId, parsedEnums.Item2, createCommand.ActivityType,
            createCommand.CompanyImageFileId, createCommand.CreatedById
            );

        await _unitOfWork.CompanyRepository.AddAsync(company);

        var companyCity = new CompanyCity(createCommand.Location, company.Id, createCommand.CityId, createCommand.CreatedById);

        await _unitOfWork.CompanyCityRepository.AddAsync(companyCity);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    private (OwnershipType, CompanySizeEnum) ParseEnums(string ownershipType, string companySize)
    {
        if (!Enum.TryParse<OwnershipType>(ownershipType, true, out var parsedOwnershipType))
            throw new ValidationException("Invalid ownership type.");

        if (!Enum.TryParse<CompanySizeEnum>(companySize, true, out var parsedCompanySize))
            throw new ValidationException("Invalid company size.");

        return (parsedOwnershipType, parsedCompanySize);
    }
}
