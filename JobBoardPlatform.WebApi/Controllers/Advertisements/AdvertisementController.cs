using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Advertisements;

[Route("api/advertisement")]
[ApiController]
[Authorize]
public class AdvertisementController : ControllerBase
{
    private readonly IAdvertisementService _advertisementService;

    public AdvertisementController(IAdvertisementService advertisementService)
    {
        _advertisementService = advertisementService;
    }

    [HttpPost]
    [Authorize(Policy = "ApprovedEmployerOnly")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> CreateAdvertisementAsync(
        [FromBody] CreateAdvertisementRequestDto createAdvertisement,
        CancellationToken cancellationToken)
    {
        await _advertisementService.CreateAdvertisementAsync(createAdvertisement, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPut("{advertisementId:guid}")]
    [Authorize(Policy = "ApprovedEmployerOnly")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> UpdateAdvertisementAsync(
        [FromRoute] Guid advertisementId,
        [FromBody] UpdateAdvertisementRequestDto updateAdvertisement,
        CancellationToken cancellationToken)
    {
        await _advertisementService.UpdateAdvertisementAsync(advertisementId, updateAdvertisement, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("by-company/{companyId:guid}")]
    [Authorize(Policy = "ApprovedEmployerOnly")]
    public async Task<IActionResult> GetAdvertisementsByCompanyAsync(
        [FromRoute] Guid companyId,
        [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _advertisementService.GetAdvertisementsByCompanyAsync(pagingRequestDto, companyId, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{advertisementId:guid}")]
    public async Task<IActionResult> GetAdvertisementInfoByIdAsync(
       [FromRoute] Guid advertisementId,
        CancellationToken cancellationToken)
    {
        var result = await _advertisementService.GetAdvertisementInfoByIdAsync(advertisementId, cancellationToken);

        return Ok(Result<AdvertisementDetailResponseDto>.Success(result));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetActiveAdvertisementsAsync(
       [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _advertisementService.GetActiveAdvertisementsAsync(pagingRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchAdvertisementsAsync(
       [FromQuery] AdvertisementSearchRequestDto searchRequestDto,
       [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _advertisementService.SearchAdvertisementsAsync(searchRequestDto, pagingRequestDto, cancellationToken);

        return Ok(result);
    }

    [HttpGet("filter")]
    [AllowAnonymous]
    public async Task<IActionResult> FilterAdvertisementsAsync(
       [FromQuery] AdvertisementFilterRequestDto filterRequestDto,
       [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _advertisementService.FilterAdvertisementsAsync(filterRequestDto, pagingRequestDto, cancellationToken);

        return Ok(result);
    }

    [HttpGet("collabration-types")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCollaborationTypes()
    {
        var result = _advertisementService.GetCollaborationTypes();

        return Ok(Result<List<EnumResponseDto>>.Success(result));
    }
}
