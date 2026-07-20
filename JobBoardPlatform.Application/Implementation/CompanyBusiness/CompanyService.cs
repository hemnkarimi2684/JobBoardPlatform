using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AttachmentInterface;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Dto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using Microsoft.Extensions.Logging;

namespace JobBoardPlatform.Application.Implementation.CompanyBusiness;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAttachmentService _attachmentService;

    private readonly IAccessControlService _accessControlService;

    private readonly ILogger<CompanyService> _logger;

    public CompanyService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAttachmentService attachmentService, IAccessControlService accessControlService, ILogger<CompanyService> logger)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _attachmentService = attachmentService;
        _accessControlService = accessControlService;
        _logger = logger;
    }

    #region Create Methods

    public async Task<Guid> CreateCompanyAsync(CreateCompanyRequestDto createCommand)
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

        //The reason the item has a .Value property is that Enum.TryParse was used;
        //the output is a nullable enum, but I am certain the enums won't reach the service as null,
        //because the DTOs are validated in the controller,
        //and an exception is thrown if the input value is null.
        var company = new Company(
            createCommand.Name, createCommand.YearOfEstablishment, createCommand.Industry,
            createCommand.AboutUs, createCommand.WebSiteAddress, createCommand.OwnershipType,
            createCommand.OwnedByUserId, createCommand.CompanySize, createCommand.ActivityType,
            null
            );

        await _unitOfWork.CompanyRepository.AddAsync(company);

        var companyCity = new CompanyCity(createCommand.Location, company.Id, createCommand.CityId);

        await _unitOfWork.CompanyCityRepository.AddAsync(companyCity);

        var saveResult = await _unitOfWork.SaveChangesAsync() > 0;

        if (!saveResult)
            throw new ValidationException("something went wring in create companu plaese try again!");

        return company.Id;
    }

    #endregion

    #region Get Methods

    public async Task<CompanyInfoResponseDto> GetCompanyInfoByOwnerIdAsync(Guid ownerId)
    {
        var companyInfo = await _unitOfWork.CompanyRepository.GetCompanyByOwnerIdAsync(c => new CompanyInfoResponseDto(
            c.Name,
            c.OwnedByUserId,
            c.YearOfEstablishment,
            c.Industry,
            c.AboutUs,
            c.WebSiteAddress,
            c.OwnershipType,
            c.CompanySize,
            c.ActivityType,
            c.CompanyImageFileId
            ),
            ownerId);

        if (companyInfo is null)
            throw new NotFoundException($"the company with this ownerId {ownerId} not found");

        return companyInfo;
    }

    #endregion

    #region Update Methods

    public async Task<bool> UpdateCompanyIdAsync(Guid companyId, UpdateCompanyInfoRequestDto updateCommand)
    {
        var companyOwnerId = await _unitOfWork.CompanyRepository.GetCompanyOwnerIdByCompanyIdAsync(companyId);

        if (companyOwnerId == null)
            throw new NotFoundException($"The company with id {companyId} was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(companyOwnerId.Value, _currentUser);

        var updateCompanyInfoResult = await _unitOfWork.CompanyRepository.UpdateCompanyInfoAsync(companyId, MapToCompanyInfoUpdate(updateCommand));

        if (!updateCompanyInfoResult)
            throw new NotFoundException($"the company with this id {companyId} not found");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task UploadCompanyImageAsync(Guid companyId, UploadCompanyImageRequestDto imageRequestDto)
    {
        if (imageRequestDto?.File is null)
            throw new ValidationException("Image file is required.");

        var company = await _unitOfWork.CompanyRepository.GetByIdAsync(companyId);

        if (company == null)
            throw new NotFoundException($"The company with id {companyId} was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(company.OwnedByUserId, _currentUser);

        //نگه داشتن ایدی عکس قبلی برای حذف شدن بعد از اپدیت عکس توسط کارفرما
        var oldImageId = company.CompanyImageFileId;
        Guid? newImageId = null;

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            newImageId = await _attachmentService.UploadAsync(imageRequestDto.File, AttachmentType.Image);

            company.UpdateImage(newImageId);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync();

            //اینجا برای این ترای کچ کذاشتم که اگه توی فلو اضافه کردن و اپدیت کردن عکس به شرکت به اکسپشن و مشکلی خورد....
            //و عکس جدیدی اپلود شده بود اما بدون اینکه به شرکت اختصاص داشته باشه اینو بیام حذف کنم 
            if (newImageId != null)
                await DeleteAttachmentAsync(newImageId.Value);

            throw;
        }

        //حالا اگه عکس جدیدی سیو شد و اپدیت شد بیا اون عکس قدیمی رو حذف کن 
        if (oldImageId != null)
            await DeleteAttachmentAsync(oldImageId.Value);
    }

    #endregion

    #region Private Methods

    private async Task DeleteAttachmentAsync(Guid attachmentId)
    {
        try
        {
            await _attachmentService.HardDeleteAttachmentAsync(attachmentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete attachment {AttachmentId}", attachmentId);
        }
    }

    private CompanyInfoUpdate MapToCompanyInfoUpdate(UpdateCompanyInfoRequestDto updateCompanyInfoCommand)
    {
        return new CompanyInfoUpdate
        (
            updateCompanyInfoCommand.Name,
            updateCompanyInfoCommand.YearOfEstablishment,
            updateCompanyInfoCommand.Industry,
            updateCompanyInfoCommand.AboutUs,
            updateCompanyInfoCommand.WebSiteAddress,
            updateCompanyInfoCommand.OwnershipType,
            updateCompanyInfoCommand.CompanySize,
            updateCompanyInfoCommand.ActivityType,
            _currentUser.UserId
        );
    }

    #endregion
}
