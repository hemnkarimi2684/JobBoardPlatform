using JobBoardPlatform.Application.Common.Dto.RequestDto.CityDto;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Admins.Cities;

[Route("api/admin/cities")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminCitiesController : ControllerBase
{
    private readonly ICityService _cityService;

    public AdminCitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpPost]
    [RequestModelValidationFilter]
    public async Task<IActionResult> CreateCityAsync(
    [FromBody] CreateCityRequestDto requestDto,
    CancellationToken cancellationToken)
    {
        await _cityService.CreateCityAsync(requestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpDelete("{cityId:guid}")]
    public async Task<IActionResult> DeleteAsync(
    [FromRoute] Guid cityId,
    CancellationToken cancellationToken)
    {
        await _cityService.SoftDeleteAsync(cityId, cancellationToken);

        return Ok(Result.Success());
    }
}
