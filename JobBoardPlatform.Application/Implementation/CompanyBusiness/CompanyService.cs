using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.Helper;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AttachmentInterface;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Dto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using Microsoft.AspNetCore.Http;
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

    public async Task<Guid> CreateCompanyAsync(
        CreateCompanyRequestDto createCommand,
        CancellationToken cancellationToken = default)
    {
        await ValidateForCreateAsync(
            createCommand.CityId,
            createCommand.JobCategoryId,
            createCommand.Name,
            createCommand.OwnedByUserId,
            cancellationToken);

        var company = new Company(
            createCommand.Name, createCommand.YearOfEstablishment,
            createCommand.AboutUs, createCommand.WebSiteAddress, createCommand.OwnershipType,
            createCommand.OwnedByUserId, createCommand.CompanySize, createCommand.JobCategoryId, createCommand.ActivityType,
            null
            );

        await _unitOfWork.CompanyRepository.AddAsync(company, cancellationToken);

        var companyCity = new CompanyCity(createCommand.Location, company.Id, createCommand.CityId);

        await _unitOfWork.CompanyCityRepository.AddAsync(companyCity, cancellationToken);

        var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!saveResult)
            throw new ValidationException("something went wring in create companu plaese try again!");

        return company.Id;
    }

    #endregion

    #region Get Methods

    public async Task<Pagination<CompanyDetailResponseDto>> GetAllCompaniesAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var (result, totalDataCount) = await _unitOfWork.CompanyRepository.GetAllCompaniesAsync(c => new CompanyDetailResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            UserId = c.OwnedByUserId,
            YearOfEstablishment = c.YearOfEstablishment,
            JobCategoryId = c.JobCategoryId,
            JobCategoryName = c.JobCategory.Name,
            AboutUs = c.AboutUs,
            WebSiteAddress = c.WebSiteAddress,
            OwnershipType = c.OwnershipType,
            CompanySize = c.CompanySize,
            ActivityType = c.ActivityType,
            CompanyImageFileId = c.CompanyImageFileId,
            Cities = c.CompanyCities.Select(cc => cc.CityId).ToList()
        },
        textRequestDto.Text,
        cancellationToken,
        pagingCommand.PageNumber,
        pagingCommand.PageSize);

        return Pagination<CompanyDetailResponseDto>.GetPagination(result, pagingCommand.PageNumber, pagingCommand.PageSize, totalDataCount);
    }

    public async Task<CompanyDetailResponseDto> GetCompanyByIdAsync(
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.CompanyRepository.GetCompanyByIdAsync(c => new CompanyDetailResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            UserId = c.OwnedByUserId,
            YearOfEstablishment = c.YearOfEstablishment,
            JobCategoryId = c.JobCategoryId,
            JobCategoryName = c.JobCategory.Name,
            AboutUs = c.AboutUs,
            WebSiteAddress = c.WebSiteAddress,
            OwnershipType = c.OwnershipType,
            CompanySize = c.CompanySize,
            ActivityType = c.ActivityType,
            CompanyImageFileId = c.CompanyImageFileId,
            Cities = c.CompanyCities.Select(cc => cc.CityId).ToList()
        },
          companyId, cancellationToken);

        if (company is null)
            throw new NotFoundException($"the company with this id {companyId} not found");

        return company;
    }

    public List<EnumResponseDto> GetCompanySizes()
    {
        var companySizes = EnumHelper.GetEnumValues<CompanySizeEnum>();

        if (companySizes is null)
            throw new NotFoundException("there is no company size in the system");

        return companySizes;
    }

    public List<EnumResponseDto> GetOwnershipTypes()
    {
        var ownerShipTypes = EnumHelper.GetEnumValues<OwnershipType>();

        if (ownerShipTypes is null)
            throw new NotFoundException("there is no ownerShip types in the system");

        return ownerShipTypes;
    }

    #endregion

    #region Update Methods

    public async Task<bool> UpdateCompanyIdAsync(
        Guid companyId,
        UpdateCompanyInfoRequestDto updateCommand,
        CancellationToken cancellationToken = default)
    {
        var companyOwnerId = await _unitOfWork.CompanyRepository.GetCompanyOwnerIdByCompanyIdAsync(companyId, cancellationToken);

        if (companyOwnerId == null)
            throw new NotFoundException($"The company with id {companyId} was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(companyOwnerId.Value, _currentUser);

        var updateCompanyInfoResult = await _unitOfWork.CompanyRepository.UpdateCompanyInfoAsync(
                                                                                companyId,
                                                                                cancellationToken,
                                                                                MapToCompanyInfoUpdate(updateCommand));

        if (!updateCompanyInfoResult)
            throw new NotFoundException($"the company with this id {companyId} not found");

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Delete Methods

    public async Task DeleteCompanyImageAsync(
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.CompanyRepository.GetByIdAsync(companyId, cancellationToken, true);

        if (company == null)
            throw new NotFoundException($"the company with id {companyId} was not found");

        _accessControlService.EnsureOwnerEmployer(company.OwnedByUserId, _currentUser);

        if (company.CompanyImageFileId == null)
            throw new ValidationException("this company does not have any image");

        var attachmentId = company.CompanyImageFileId.Value;

        company.UpdateImage(null);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            //ذخیره تغییر شرکت فیلد عکس 
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // حذف سخت فایل و رکورد اتچمنت از دیتابیس
            var deleted = await _attachmentService.HardDeleteAttachmentAsync(attachmentId, cancellationToken);

            if (!deleted)
                throw new InvalidOperationException("Failed to delete the attachment file or database record.");

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            _logger.LogError(ex, "Failed to delete company image for CompanyId: {CompanyId}, AttachmentId: {AttachmentId}. Transaction rolled back.",
                companyId, attachmentId);

            throw;
        }
    }

    #endregion

    #region Upload Company Image

    public async Task UploadCompanyImageAsync(
        Guid companyId,
        UploadCompanyImageRequestDto imageRequestDto,
        CancellationToken cancellationToken = default)
    {
        if (imageRequestDto?.Image is null)
            throw new ValidationException("Image file is required.");

        var company = await _unitOfWork.CompanyRepository.GetByIdAsync(companyId, cancellationToken, true);

        if (company == null)
            throw new NotFoundException($"The company with id {companyId} was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(company.OwnedByUserId, _currentUser);

        await UploadImageAsync(company, imageRequestDto.Image, cancellationToken);
    }

    #endregion

    #region DownLoad Company Image


    public async Task<AttachmentResponseDto> DownloadCompanyImageAsync(
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.CompanyRepository.GetByIdAsync(companyId, cancellationToken);

        if (company == null)
            throw new NotFoundException($"The company with id {companyId} was not found.");

        if (company.CompanyImageFileId == null)
            throw new NotFoundException($"The company with id '{companyId}' does not have an attached image.");

        return await _attachmentService.DownloadAsync(company.CompanyImageFileId.Value, cancellationToken);
    }


    #endregion

    #region Private Methods

    private async Task DeleteAttachmentAsync(Guid attachmentId, CancellationToken cancellationToken)
    {
        try
        {
            await _attachmentService.HardDeleteAttachmentAsync(attachmentId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete attachment {AttachmentId}", attachmentId);
        }
    }

    private async Task UploadImageAsync(Company company, IFormFile image, CancellationToken cancellationToken)
    {
        //نگه داشتن ایدی عکس قبلی برای حذف شدن بعد از اپدیت عکس توسط کارفرما
        var oldImageId = company.CompanyImageFileId;
        Guid? newImageId = null;

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            newImageId = await _attachmentService.UploadAsync(image, AttachmentType.Image, cancellationToken);

            company.UpdateImage(newImageId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            //اینجا برای این ترای کچ کذاشتم که اگه توی فلو اضافه کردن و اپدیت کردن عکس به شرکت به اکسپشن و مشکلی خورد....
            //و عکس جدیدی اپلود شده بود اما بدون اینکه به شرکت اختصاص داشته باشه اینو بیام حذف کنم 
            if (newImageId != null)
                await DeleteAttachmentAsync(newImageId.Value, cancellationToken);

            throw;
        }

        //حالا اگه عکس جدیدی سیو شد و اپدیت شد بیا اون عکس قدیمی رو حذف کن 
        if (oldImageId != null)
            await DeleteAttachmentAsync(oldImageId.Value, cancellationToken);
    }

    private CompanyInfoUpdate MapToCompanyInfoUpdate(UpdateCompanyInfoRequestDto updateCompanyInfoCommand)
    {
        return new CompanyInfoUpdate
        (
            updateCompanyInfoCommand.Name,
            updateCompanyInfoCommand.YearOfEstablishment,
            updateCompanyInfoCommand.JobCategoryId,
            updateCompanyInfoCommand.AboutUs,
            updateCompanyInfoCommand.WebSiteAddress,
            updateCompanyInfoCommand.OwnershipType,
            updateCompanyInfoCommand.CompanySize,
            updateCompanyInfoCommand.ActivityType,
            _currentUser.UserId
        );
    }

    private async Task ValidateForCreateAsync(Guid cityId, Guid jobCategoryId, string companyName, Guid ownedByUserId, CancellationToken cancellationToken)
    {
        var doesCityExist = await _unitOfWork.CityRepository.IsCityExistAsync(cityId, cancellationToken);

        if (!doesCityExist)
            throw new NotFoundException($"City with id {cityId} was not found.");

        var doesJobCategoryExist = await _unitOfWork.JobCategoryRepository.ExistAsync(jobCategoryId, cancellationToken);

        if (!doesJobCategoryExist)
            throw new NotFoundException($"the job category with id {jobCategoryId} was not found.");

        var companyExistsByName = await _unitOfWork.CompanyRepository.IsCompanyExistByNameAsync(companyName, cancellationToken);

        if (companyExistsByName)
            throw new ConflictException($"the company with this name {companyName} already exist");

        var companyExistsForOwner = await _unitOfWork.CompanyRepository.IsCompanyExistForOwnerId(ownedByUserId, cancellationToken);

        if (companyExistsForOwner)
            throw new ConflictException($"this owner already has company");
    }

    #endregion
}
