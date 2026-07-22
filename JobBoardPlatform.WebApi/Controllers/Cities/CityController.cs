using JobBoardPlatform.Application.Common.Dto.RequestDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Cities;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpPost]
    [Authorize("Admin")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> CreateCityAsync(
        [FromBody] CreateCityRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        await _cityService.CreateCityAsync(requestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("{cityId:guid}/companies")]
    public async Task<IActionResult> GetCityCompaniesAsync(
        [FromRoute] Guid cityId,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _cityService.GetCityCompaniesAsync(cityId, pagingRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("by-province/{provinceId:guid}")]
    public async Task<IActionResult> GetProvinceCitiesAsync(
        [FromRoute] Guid provinceId,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _cityService.GetProvinceCitiesAsync(provinceId, pagingRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCitiesAsync(
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _cityService.GetAllCitiesAsync(pagingRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{cityId:guid}")]
    public async Task<IActionResult> GetCityByIdAsync(
        [FromRoute] Guid cityId,
        CancellationToken cancellationToken)
    {
        var result = await _cityService.GetCityByIdAsync(cityId, cancellationToken);

        return Ok(Result<CityDetailResponseDto>.Success(result));
    }
}
