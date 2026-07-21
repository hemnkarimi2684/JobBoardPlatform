using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
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

    [HttpGet("{ownerId:guid}")]
    // [Authorize(Roles = "Employer,Admin")]
    public async Task<IActionResult> GetCompanyInfoByOwnerIdAsync(
        [FromRoute] Guid ownerId,
        CancellationToken cancellationToken)
    {
        var result = await _companyService.GetCompanyInfoByOwnerIdAsync(ownerId, cancellationToken);

        return Ok(Result<CompanyInfoResponseDto>.Success(result));
    }

    [HttpPut("{companyId:guid}")]
    //[Authorize(Roles = "Employer")]
    public async Task<IActionResult> UpdateCompanyIdAsync(
        [FromRoute] Guid companyId,
        [FromBody] UpdateCompanyInfoRequestDto update,
        CancellationToken cancellationToken)
    {
        await _companyService.UpdateCompanyIdAsync(companyId, update, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{companyId:guid}/upload-image")]
    //[Authorize(Roles = "Employer")]
    public async Task<IActionResult> UploadCompanyImageAsync(
        [FromRoute] Guid companyId,
        [FromForm] UploadCompanyImageRequestDto imageRequestDto,
        CancellationToken cancellationToken)
    {
        await _companyService.UploadCompanyImageAsync(companyId, imageRequestDto, cancellationToken);

        return Ok(Result.Success());
    }
}
