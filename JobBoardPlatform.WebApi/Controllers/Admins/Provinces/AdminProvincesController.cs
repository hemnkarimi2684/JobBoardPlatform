using JobBoardPlatform.Application.Common.Dto.RequestDto.ProvinceDto;
using JobBoardPlatform.Application.Interfaces.ProvinceInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Admins.Provinces;

[Route("api/admin/provinces")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminProvincesController : ControllerBase
{
    private readonly IProvinceService _provinceService;

    public AdminProvincesController(IProvinceService provinceService)
    {
        _provinceService = provinceService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProvinceAsync(
       [FromBody] CreateProvinceRequestDto provinceRequestDto,
        CancellationToken cancellationToken)
    {
        await _provinceService.CreateProvinceAsync(provinceRequestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpDelete("{provinceId:guid}")]
    public async Task<IActionResult> DeleteAsync(
       [FromRoute] Guid provinceId,
        CancellationToken cancellationToken)
    {
        await _provinceService.SoftDeleteAsync(provinceId, cancellationToken);

        return Ok(Result.Success());
    }

}
