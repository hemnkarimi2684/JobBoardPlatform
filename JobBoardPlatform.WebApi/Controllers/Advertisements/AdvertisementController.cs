using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Application.Implementation.AdvertisementBusiness;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Advertisements;

[Route("api/[controller]s")]
[ApiController]
public class AdvertisementController : ControllerBase
{
    private readonly AdvertisementService _advertisementService;

    public AdvertisementController(AdvertisementService advertisementService)
    {
        _advertisementService = advertisementService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdvertisementAsync([FromBody] CreateAdvertisementRequestDto createAdvertisement)
    {
        await _advertisementService.CreateAdvertisementAsync(createAdvertisement);

        return NoContent();
    }

    [HttpPut("{advertisementId:guid}")]
    public async Task<IActionResult> UpdateAdvertisementAsync([FromRoute] Guid advertisementId, [FromBody] UpdateAdvertisementRequestDto updateAdvertisement)
    {
        await _advertisementService.UpdateAdvertisementAsync(advertisementId, updateAdvertisement);

        return Ok(Result.Success());
    }

    [HttpDelete("{advertisementId:guid}")]
    public async Task<IActionResult> SoftDeleteAdvertisementAsync([FromRoute] Guid advertisementId)
    {
        var result = await _advertisementService.SoftDeleteAdvertisementAsync(advertisementId);

        return NoContent();
    }

    [HttpGet("by-company")]
    public async Task<IActionResult> GetAdvertisementsByCompanyAsync([FromQuery] Guid companyId, [FromQuery] PagingRequestDto pagingRequestDto)
    {
        var result = await _advertisementService.GetAdvertisementsByCompanyAsync(pagingRequestDto, companyId);

        return Ok(Result<Pagination<AdvertisementDetailResponseDto>>.Success(result));
    }

    [HttpPatch("in-activate/{advertisementId:guid}")]
    public async Task<IActionResult> InActivateAdvertisementAsync([FromRoute] Guid advertisementId)
    {
        await _advertisementService.InActivateAdvertisementAsync(advertisementId);

        return Ok(Result.Success());
    }

    [HttpPatch("activate/{advertisementId:guid}")]
    public async Task<IActionResult> ActivateAdvertisementAsync([FromRoute] Guid advertisementId)
    {
        var result = await _advertisementService.ActivateAdvertisementAsync(advertisementId);

        return Ok(Result.Success());
    }

    [HttpGet("{advertisementId:guid}")]
    public async Task<IActionResult> GetAdvertisementInfoByIdAsync(Guid advertisementId)
    {
        var result = await _advertisementService.GetAdvertisementInfoByIdAsync(advertisementId);

        return Ok(Result<AdvertisementDetailResponseDto>.Success(result));
    }

}
