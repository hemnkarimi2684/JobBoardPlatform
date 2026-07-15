using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Companies;

[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyRequestDto createCompany)
    {
        await _companyService.CreateCompanyAsync(createCompany);

        return NoContent();
    }

    [HttpGet("{ownerId:guid}")]
    public async Task<IActionResult> GetCompanyInfoByOwnerIdAsync([FromRoute] Guid ownerId)
    {
        var result = await _companyService.GetCompanyInfoByOwnerIdAsync(ownerId);

        return Ok(Result<CompanyInfoResponseDto>.Success(result));
    }

    [HttpPut("{companyId:guid}")]
    public async Task<IActionResult> UpdateCompanyIdAsync([FromRoute] Guid companyId, [FromBody] UpdateCompanyInfoRequestDto update)
    {
        await _companyService.UpdateCompanyIdAsync(companyId, update);

        return Ok(Result.Success());
    }
}
