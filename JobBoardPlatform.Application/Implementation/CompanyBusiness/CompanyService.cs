using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.CompanyDto.Command;
using JobBoardPlatform.Application.Common.Dto.CompanyDto.Result;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Dto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;

namespace JobBoardPlatform.Application.Implementation.CompanyBusiness;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    public CompanyService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<bool> CreateCompanyAsync(CreateCompanyCommand createCommand)
    {
        CheckEmployerOrAdminPermission(_currentUser);

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

        //The reason the item has a .Value property is that Enum.TryParse was used;
        //the output is a nullable enum, but I am certain the enums won't reach the service as null,
        //because the DTOs are validated in the controller,
        //and an exception is thrown if the input value is null.
        var company = new Company(
            createCommand.Name, createCommand.YearOfEstablishment, createCommand.Industry,
            createCommand.AboutUs, createCommand.WebSiteAddress, parsedEnums.Item1.Value,
            createCommand.OwnedByUserId, parsedEnums.Item2.Value, createCommand.ActivityType,
            createCommand.CompanyImageFileId, _currentUser.UserId
            );

        await _unitOfWork.CompanyRepository.AddAsync(company);

        var companyCity = new CompanyCity(createCommand.Location, company.Id, createCommand.CityId, _currentUser.UserId);

        await _unitOfWork.CompanyCityRepository.AddAsync(companyCity);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<CompanyInfoResult> GetCompanyInfoByOwnerIdAsync(Guid ownerId)
    {
        var companyInfo = await _unitOfWork.CompanyRepository.GetCompanyByOwnerIdAsync(c => new CompanyInfoResult(
            c.Name,
            c.YearOfEstablishment,
            c.Industry,
            c.AboutUs,
            c.WebSiteAddress,
            c.OwnershipType,
            c.CompanySize,
            c.ActivityType
            ),
            ownerId);

        if (companyInfo is null)
            throw new NotFoundException($"the company with this ownerId {ownerId} not found");

        return companyInfo;
    }

    public async Task<bool> UpdateCompanyIdAsync(Guid companyId, UpdateCompanyInfoCommand updateCommand)
    {
        var companyOwnerId = await _unitOfWork.CompanyRepository.GetCompanyOwnerIdByCompanyIdAsync(companyId);

        if (companyOwnerId == null)
            throw new NotFoundException($"The company with id {companyId} was not found.");

        CheckOwnerOrAdminPermission(companyOwnerId, _currentUser);

        var updateCompanyInfoResult = await _unitOfWork.CompanyRepository.UpdateCompanyInfoAsync(companyId, MapToCompanyInfoUpdate(updateCommand));

        if (!updateCompanyInfoResult)
            throw new NotFoundException($"the company with this id {companyId} not found");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    private (OwnershipType?, CompanySizeEnum?) ParseEnums(string? ownershipType, string? companySize)
    {
        OwnershipType? parsedOwnershipType = null;
        CompanySizeEnum? parsedCompanySize = null;

        if (!string.IsNullOrWhiteSpace(ownershipType))
        {
            if (!Enum.TryParse<OwnershipType>(ownershipType, true, out var result))
                throw new ValidationException("Invalid ownership type.");

            parsedOwnershipType = result;
        }

        if (!string.IsNullOrWhiteSpace(companySize))
        {
            if (!Enum.TryParse<CompanySizeEnum>(companySize, true, out var result))
                throw new ValidationException("Invalid company size.");

            parsedCompanySize = result;
        }

        return (parsedOwnershipType, parsedCompanySize);
    }

    private CompanyInfoUpdate MapToCompanyInfoUpdate(UpdateCompanyInfoCommand updateCompanyInfoCommand)
    {
        var parsedEnums = ParseEnums(updateCompanyInfoCommand.OwnershipType, updateCompanyInfoCommand.CompanySize);

        return new CompanyInfoUpdate
        (
            updateCompanyInfoCommand.Name,
            updateCompanyInfoCommand.YearOfEstablishment,
            updateCompanyInfoCommand.Industry,
            updateCompanyInfoCommand.AboutUs,
            updateCompanyInfoCommand.WebSiteAddress,
            parsedEnums.Item1,
            parsedEnums.Item2,
            updateCompanyInfoCommand.ActivityType,
            _currentUser.UserId
        );
    }

    private void CheckOwnerOrAdminPermission(Guid? ownerId, ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isOwner = ownerId == currentUser.UserId;

        var isAdmin = currentUser.UserRoles.Contains(RoleConstants.AdminRoleName);

        var isEmployer = currentUser.UserRoles.Contains(RoleConstants.EmployerRoleName);
                                                                
        if (!isAdmin && !(isOwner && isEmployer))
            throw new ForbiddenException("You do not have sufficient access to manage this company.");
    }

    private void CheckEmployerOrAdminPermission(ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isAdminOrEmployer = currentUser.UserRoles.Any(role => role == RoleConstants.EmployerRoleName || role == RoleConstants.AdminRoleName);

        if (!isAdminOrEmployer)
            throw new ForbiddenException("You do not have sufficient access to manage a company.");
    }
}
