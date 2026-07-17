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
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyRequestDto createCompany)
    {
        await _companyService.CreateCompanyAsync(createCompany);

        return Ok(Result.Success());
    }

    [HttpGet("{ownerId:guid}")]
    [Authorize(Roles = "Employer,Admin")]
    public async Task<IActionResult> GetCompanyInfoByOwnerIdAsync([FromRoute] Guid ownerId)
    {
        var result = await _companyService.GetCompanyInfoByOwnerIdAsync(ownerId);

        return Ok(Result<CompanyInfoResponseDto>.Success(result));
    }

    [HttpPut("{companyId:guid}")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> UpdateCompanyIdAsync([FromRoute] Guid companyId, [FromBody] UpdateCompanyInfoRequestDto update)
    {
        await _companyService.UpdateCompanyIdAsync(companyId, update);

        return Ok(Result.Success());
    }

    [HttpPatch("{companyId:guid}/image")]
    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> UploadCompanyImageAsync([FromRoute] Guid companyId, [FromForm] UploadCompanyImageRequestDto imageRequestDto)
    {
        await _companyService.UploadCompanyImageAsync(companyId, imageRequestDto);

        return Ok(Result.Success());
    }
}
