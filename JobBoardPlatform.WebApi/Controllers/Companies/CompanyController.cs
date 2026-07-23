using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Companies;

[Route("api/companies")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompanyAsync(
        [FromBody] CreateCompanyRequestDto createCompany,
        CancellationToken cancellationToken)
    {
        await _companyService.CreateCompanyAsync(createCompany, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("by-user/{ownerId:guid}")]
    [Authorize(Roles = "Employer,Admin")]
    public async Task<IActionResult> GetCompanyProfileByOwnerIdAsync(
        [FromRoute] Guid ownerId,
        CancellationToken cancellationToken)
    {
        var result = await _companyService.GetCompanyProfileByOwnerIdAsync(ownerId, cancellationToken);

        return Ok(Result<CompanyProfileResponseDto>.Success(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCompaniesAsync(
        [FromQuery] TextRequestDto textRequestDto,
        [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _companyService.GetAllCompaniesAsync(textRequestDto, pagingRequestDto, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{companyId:guid}")]
    public async Task<IActionResult> GetCompanyByIdAsync(
        [FromRoute] Guid companyId,
        CancellationToken cancellationToken)
    {
        var result = await _companyService.GetCompanyByIdAsync(companyId, cancellationToken);

        return Ok(Result<CompanyProfileResponseDto>.Success(result));
    }

    [HttpPut("{companyId:guid}")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> UpdateCompanyIdAsync(
        [FromRoute] Guid companyId,
        [FromBody] UpdateCompanyInfoRequestDto update,
        CancellationToken cancellationToken)
    {
        await _companyService.UpdateCompanyIdAsync(companyId, update, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{companyId:guid}/upload-image")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> UploadCompanyImageAsync(
        [FromRoute] Guid companyId,
        [FromForm] UploadCompanyImageRequestDto imageRequestDto,
        CancellationToken cancellationToken)
    {
        await _companyService.UploadCompanyImageAsync(companyId, imageRequestDto, cancellationToken);

        return Ok(Result.Success());
    }
}
