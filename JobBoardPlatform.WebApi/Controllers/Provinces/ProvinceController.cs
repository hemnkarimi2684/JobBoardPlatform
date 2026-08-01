using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ProvinceDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ProvinceDto;
using JobBoardPlatform.Application.Interfaces.ProvinceInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Provinces;

[Route("api/provinces")]
[ApiController]
[Authorize]
public class ProvinceController : ControllerBase
{
    private readonly IProvinceService _provinceService;

    public ProvinceController(IProvinceService provinceService)
    {
        _provinceService = provinceService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllProvincesAsync(
      [FromQuery] TextRequestDto textRequestDto,
      [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _provinceService.GetAllProvincesAsync(textRequestDto, pagingRequestDto, cancellationToken);

        return Ok(result);
    }
}
