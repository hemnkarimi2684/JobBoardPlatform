using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Companies;

[Route("api/companies")]
[ApiController]
[Authorize]
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

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCompaniesAsync(
        [FromQuery] TextRequestDto textRequestDto,
        [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _companyService.GetAllCompaniesAsync(textRequestDto, pagingRequestDto, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{companyId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCompanyByIdAsync(
        [FromRoute] Guid companyId,
        CancellationToken cancellationToken)
    {
        var result = await _companyService.GetCompanyByIdAsync(companyId, cancellationToken);

        return Ok(Result<CompanyDetailResponseDto>.Success(result));
    }

    [HttpPut("{companyId:guid}")]
    [Authorize(Policy = "ApprovedEmployerOnly")]
    public async Task<IActionResult> UpdateCompanyIdAsync(
        [FromRoute] Guid companyId,
        [FromBody] UpdateCompanyInfoRequestDto update,
        CancellationToken cancellationToken)
    {
        await _companyService.UpdateCompanyIdAsync(companyId, update, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{companyId:guid}/upload-image")]
    [Authorize(Policy = "ApprovedEmployerOnly")]
    public async Task<IActionResult> UploadCompanyImageAsync(
        [FromRoute] Guid companyId,
        [FromForm] UploadCompanyImageRequestDto imageRequestDto,
        CancellationToken cancellationToken)
    {
        await _companyService.UploadCompanyImageAsync(companyId, imageRequestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("{companyId:guid}/download-image")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadCompanyImageAsync(
        [FromRoute] Guid companyId,
        CancellationToken cancellationToken)
    {
        var result = await _companyService.DownloadCompanyImageAsync(companyId, cancellationToken);

        return File(result.Data, result.ContentType, result.FileName);
    }

    [HttpGet("owner-ship-types")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOwnershipTypes()
    {
        var result = _companyService.GetOwnershipTypes();

        return Ok(Result<List<EnumResponseDto>>.Success(result));
    }

    [HttpGet("company-sizes")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCompanySizes()
    {
        var result = _companyService.GetCompanySizes();

        return Ok(Result<List<EnumResponseDto>>.Success(result));
    }
}
